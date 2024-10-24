import sqlite3 from 'sqlite3';

export function initializeDatabase(db: sqlite3.Database): void {
  // Create tables
  db.run(`CREATE TABLE IF NOT EXISTS jobAd (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    textEN TEXT NOT NULL
  )`);

  db.run(`CREATE TABLE IF NOT EXISTS jobAdTranslation (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    jobAdId INTEGER NOT NULL,
    language TEXT NOT NULL,
    translatedText TEXT NOT NULL
  )`);

  // Insert demo job ads
  const demoAds = [
    {
      title: 'Junior Software Engineer',
      textEN:
        'We are seeking a talented Junior Software Engineer to join our dynamic team. The ideal candidate should have a strong foundation in programming concepts, be eager to learn, and possess excellent problem-solving skills. Familiarity with languages such as JavaScript, Python, or Java is a plus.',
      translationDE:
        'Wir suchen talentierte Junior Software Engineers zur Verstärkung unseres dynamischen Teams. Ideale Bewerbende sollten über fundierte Kenntnisse in Programmierkonzepten verfügen, lernbegierig sein und ausgezeichnete Problemlösungsfähigkeiten besitzen. Vertrautheit mit Sprachen wie JavaScript, Python oder Java ist von Vorteil.',
    },
    {
      title: 'UX Designer',
      textEN:
        'We are looking for a creative and user-focused UX Designer to help create intuitive and engaging user experiences for our products. The ideal candidate should have a strong portfolio demonstrating their design process, proficiency in design tools, and the ability to collaborate effectively with cross-functional teams.',
      translationDE:
        'Wir suchen kreative und nutzerorientierte UX-Designfachkräfte, die uns dabei helfen, intuitive und ansprechende Benutzererlebnisse für unsere Produkte zu schaffen.',
    },
    {
      title: 'Office Manager',
      textEN:
        'We are seeking an organized and proactive Office Manager to oversee daily operations and ensure the smooth running of our office. The ideal candidate should have excellent communication skills, experience in office management, and the ability to multitask effectively.',
      // No German translation provided
    },
  ];

  demoAds.forEach((ad, index) => {
    // Check if the job ad already exists
    db.get('SELECT id FROM jobAd WHERE title = ? AND textEN = ?', [ad.title, ad.textEN], (err, row) => {
      if (err) {
        console.error('Error checking for existing job ad:', err.message);
        return;
      }

      if (row) {
        console.log(`Demo ad ${index + 1} already exists, skipping insertion`);
        return;
      }

      // Insert the job ad if it doesn't exist
      db.run('INSERT INTO jobAd (title, textEN) VALUES (?, ?)', [ad.title, ad.textEN], function (err) {
        if (err) {
          console.error('Error inserting job ad:', err.message);
          return;
        }
        const jobAdId = this.lastID;

        // Check if the translation already exists
        if (ad.translationDE) {
          db.get('SELECT id FROM jobAdTranslation WHERE jobAdId = ? AND language = ?', [jobAdId, 'DE'], (err, row) => {
            if (err) {
              console.error('Error checking for existing translation:', err.message);
              return;
            }

            if (row) {
              console.log(`Translation for demo ad ${index + 1} already exists, skipping insertion`);
              return;
            }

            // Insert the translation if it doesn't exist
            db.run(
              'INSERT INTO jobAdTranslation (jobAdId, language, translatedText) VALUES (?, ?, ?)',
              [jobAdId, 'DE', ad.translationDE],
              function (err) {
                if (err) {
                  console.error('Error inserting job ad translation:', err.message);
                } else {
                  console.log(`Demo ad ${index + 1} and its translation inserted successfully`);
                }
              },
            );
          });
        }
      });
    });
  });
}
