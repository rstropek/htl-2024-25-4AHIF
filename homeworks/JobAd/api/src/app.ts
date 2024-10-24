import express from 'express';
import cors from 'cors';
import sqlite3 from 'sqlite3';
import swaggerUi from 'swagger-ui-express';
import { initializeDatabase } from './database.js';
import { extendZodWithOpenApi, OpenApiGeneratorV31, OpenAPIRegistry } from '@asteasolutions/zod-to-openapi';
import {
  JobAd,
  jobAdSchema,
  jobAdTranslationSchema,
  jobAdTranslationWithIdSchema,
  jobAdWithIdSchema,
  JobAdWithTranslations,
  jobAdWithTranslationsSchema,
  translatedTextResponseSchema,
  translateRequestSchema,
} from './schema.js';
import { z } from 'zod';
import dotenv from 'dotenv';
import * as deepl from 'deepl-node';

dotenv.config();

const app = express();
const port = 3000;

app.use(cors());
app.use(express.json());

const db = new sqlite3.Database('./ads.db', (err) => {
  if (err) {
    console.error('Could not connect to SQLite database', err);
  } else {
    console.log('Connected to SQLite database');
    initializeDatabase(db);
  }
});

extendZodWithOpenApi(z);
const registry = new OpenAPIRegistry();

registry.registerPath({
  method: 'get',
  path: '/ads',
  summary: 'Get all job ads',
  responses: {
    200: {
      description: 'A list of job ads',
      content: {
        'application/json': {
          schema: jobAdWithTranslationsSchema,
        },
      },
    },
  },
});
app.get('/ads', (req, res) => {
  db.all('SELECT * FROM jobAd', (err, rows: JobAd[]) => {
    res.json(rows);
  });
});

registry.registerPath({
  method: 'get',
  path: '/ads/{id}',
  summary: 'Get a job ad by id',
  request: {
    params: z.object({
      id: z.string().openapi({}),
    }),
  },
  responses: {
    200: {
      description: 'A job ad',
      content: {
        'application/json': {
          schema: jobAdWithTranslationsSchema,
        },
      },
    },
    404: {
      description: 'Job ad not found',
    },
  },
});
app.get('/ads/:id', (req, res) => {
  const { id } = req.params;
  db.all(
    'SELECT a.id, a.title, a.textEN, t.language, t.translatedText FROM jobAd a LEFT JOIN jobAdTranslation t ON a.id = t.jobAdId WHERE a.id = ?',
    [id],
    (err, rows: { id: number; title: string; textEN: string; language?: string; translatedText?: string }[]) => {
      if (err) {
        return res.status(500).json({ error: 'Database error' });
      }

      if (rows.length === 0) {
        return res.status(404).json({ error: 'Job ad not found' });
      }

      const result: JobAdWithTranslations = {
        id: rows[0].id,
        title: rows[0].title,
        textEN: rows[0].textEN,
        translations: [],
      };

      if (rows[0].language && rows[0].translatedText) {
        result.translations = rows.map((row) => ({
          language: row.language ?? '',
          translatedText: row.translatedText ?? '',
        }));
      }

      res.json(result);
    },
  );
});

// Add a new job ad
registry.registerPath({
  method: 'post',
  path: '/ads',
  summary: 'Add a new job ad',
  request: {
    body: {
      content: {
        'application/json': {
          schema: jobAdSchema,
        },
      },
    },
  },
  responses: {
    201: {
      description: 'Job ad created successfully',
      content: {
        'application/json': {
          schema: jobAdWithIdSchema,
        },
      },
    },
    400: {
      description: 'Invalid input',
    },
  },
});
app.post('/ads', (req, res) => {
  const { title, textEN } = req.body;
  db.run('INSERT INTO jobAd (title, textEN) VALUES (?, ?)', [title, textEN], function (err) {
    if (err) {
      res.status(400).json({ error: 'Failed to create job ad' });
    } else {
      res.status(201).json({ id: this.lastID, title, textEN });
    }
  });
});

// Delete a job ad by id
registry.registerPath({
  method: 'delete',
  path: '/ads/{id}',
  summary: 'Delete a job ad by id',
  request: {
    params: z.object({
      id: z.string().openapi({}),
    }),
  },
  responses: {
    204: {
      description: 'Job ad deleted successfully',
    },
    404: {
      description: 'Job ad not found',
    },
  },
});
app.delete('/ads/:id', (req, res) => {
  const { id } = req.params;
  db.run('DELETE FROM jobAd WHERE id = ?', [id], function (err) {
    if (err) {
      res.status(500).json({ error: 'Failed to delete job ad' });
    } else if (this.changes === 0) {
      res.status(404).json({ error: 'Job ad not found' });
    } else {
      res.status(204).send();
    }
  });
});

// Update a job ad by id
registry.registerPath({
  method: 'patch',
  path: '/ads/{id}',
  summary: 'Update a job ad by id',
  request: {
    params: z.object({
      id: z.string().openapi({}),
    }),
    body: {
      content: {
        'application/json': {
          schema: z.object({
            title: z.string().optional(),
            textEN: z.string().optional(),
          }),
        },
      },
    },
  },
  responses: {
    200: {
      description: 'Job ad updated successfully',
      content: {
        'application/json': {
          schema: jobAdWithIdSchema,
        },
      },
    },
    400: {
      description: 'Invalid input',
    },
    404: {
      description: 'Job ad not found',
    },
  },
});

app.patch('/ads/:id', (req, res) => {
  const { id } = req.params;
  const { title, textEN } = req.body;

  if (!title && !textEN) {
    return res.status(400).json({ error: 'No fields to update' });
  }

  const fieldsToUpdate = [];
  const values = [];

  if (title) {
    fieldsToUpdate.push('title = ?');
    values.push(title);
  }

  if (textEN) {
    fieldsToUpdate.push('textEN = ?');
    values.push(textEN);
  }

  values.push(id);

  const query = `UPDATE jobAd SET ${fieldsToUpdate.join(', ')} WHERE id = ?`;

  db.run(query, values, function (err) {
    if (err) {
      return res.status(500).json({ error: 'Failed to update job ad' });
    }

    if (this.changes === 0) {
      return res.status(404).json({ error: 'Job ad not found' });
    }

    db.get('SELECT * FROM jobAd WHERE id = ?', [id], (err, row) => {
      if (err || !row) {
        return res.status(404).json({ error: 'Job ad not found' });
      }

      res.json(row);
    });
  });
});

// Upsert a job ad translation
registry.registerPath({
  method: 'put',
  path: '/ads/{id}/translations/{language}',
  summary: 'Upsert a translation for a job ad',
  request: {
    params: z.object({
      id: z.string().openapi({}),
      language: z.string().openapi({}),
    }),
    body: {
      content: {
        'application/json': {
          schema: z.object({
            translatedText: z.string(),
          }),
        },
      },
    },
  },
  responses: {
    200: {
      description: 'Translation upserted successfully',
      content: {
        'application/json': {
          schema: jobAdTranslationWithIdSchema,
        },
      },
    },
    400: {
      description: 'Invalid input',
    },
    404: {
      description: 'Job ad not found',
    },
  },
});

app.put('/ads/:id/translations/:language', (req, res) => {
  const { id, language } = req.params;
  const { translatedText } = req.body;

  db.get('SELECT id FROM jobAd WHERE id = ?', [id], (err, row) => {
    if (err || !row) {
      return res.status(404).json({ error: 'Job ad not found' });
    }

    db.get('SELECT id FROM jobAdTranslation WHERE jobAdId = ? AND language = ?', [id, language], (err, translationRow: { id: number }) => {
      if (err) {
        return res.status(500).json({ error: 'Database error' });
      }

      if (translationRow) {
        // Update existing translation
        db.run('UPDATE jobAdTranslation SET translatedText = ? WHERE id = ?', [translatedText, translationRow.id], function (err) {
          if (err) {
            return res.status(500).json({ error: 'Failed to update translation' });
          }
          res.json({ id: translationRow.id, jobAdId: Number(id), language, translatedText });
        });
      } else {
        // Insert new translation
        db.run('INSERT INTO jobAdTranslation (jobAdId, language, translatedText) VALUES (?, ?, ?)', [id, language, translatedText], function (err) {
          if (err) {
            return res.status(500).json({ error: 'Failed to insert translation' });
          }
          res.json({ id: this.lastID, jobAdId: Number(id), language, translatedText });
        });
      }
    });
  });
});

// Delete a job ad translation by job ad id and language
registry.registerPath({
  method: 'delete',
  path: '/ads/{id}/translations/{language}',
  summary: 'Delete a translation for a job ad by id and language',
  request: {
    params: z.object({
      id: z.string().openapi({}),
      language: z.string().openapi({}),
    }),
  },
  responses: {
    204: {
      description: 'Translation deleted successfully',
    },
    404: {
      description: 'Translation not found',
    },
  },
});

app.delete('/ads/:id/translations/:language', (req, res) => {
  const { id, language } = req.params;
  db.run('DELETE FROM jobAdTranslation WHERE jobAdId = ? AND language = ?', [id, language], function (err) {
    if (err) {
      return res.status(500).json({ error: 'Failed to delete translation' });
    }
    if (this.changes === 0) {
      return res.status(404).json({ error: 'Translation not found' });
    }
    res.status(204).send();
  });
});

const translator = new deepl.Translator(process.env.DEEPL_API_KEY!);
registry.registerPath({
  method: 'post',
  path: '/deepl/v2/translate',
  summary: 'Translate text',
  request: {
    body: {
      content: {
        'application/json': {
          schema: translateRequestSchema,
        },
      },
    },
  },
  responses: {
    200: {
      description: 'Translated text',
      content: {
        'application/json': {
          schema: translatedTextResponseSchema,
        },
      },
    },
  },
});
app.post('/deepl/v2/translate', async (req, res) => {
  const { text, source_lang, target_lang } = req.body;
  const result = await translator.translateText(text, source_lang, target_lang) as deepl.TextResult;
  res.json({ translation: result.text });
});

const generator = new OpenApiGeneratorV31(registry.definitions);
const specs = generator.generateDocument({
  openapi: '3.1.0',
  info: {
    title: 'Job Ads API',
    version: '1.0.0',
  },
  servers: [{ url: '/' }],
});

app.use('/swagger', swaggerUi.serve, swaggerUi.setup(specs));

app.get('/', (req, res) => {
  res.redirect('/swagger');
});

app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
