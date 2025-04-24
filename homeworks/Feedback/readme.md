# Feedback App

## Background

At this time in the course, you have learned how to develop frontend apps using Angular, create RESTful APIs using ASP.NET Core Minimal APIs, and store data in a relational database using Entity Framework Core. In this project, you will combine all of these skills to create and deploy a full-stack application.

## Project Description

The app should enable visitors to submit feedback about a course they took. Here is a feedback diagram describing the workflow:

[![](https://mermaid.ink/img/pako:eNp1U02P2jAQ_SuWzykQEhKIKiREt1y2Eu2ylyoXY08Sa4Od2g4tu-K_d5zwkVW3OTkz7z3Pmxm_Ua4F0Ixa-NWC4vBFstKwQ64Ifg0zTnLZMOXIijBLvgKIPeMvZNU0_0LWG49ZoyDZgALDnDY9inE8kp1P7wyTmBvGtz6-vQv1ud2n5XK9ya5SQIrr5c8_Hu3nvRkvCyQPCrA9cb1B5i4jj9I6ogvSgLFasVq-guhpQ6H3nCfgBhzhujUWiO9Nn1caC9BHQBPBNkOPXd6xF7CkqRm_wGqtG_KAsNOwrrsfpG4v1byz0yO2iFghwuijFAO_rZWq_AAPStylkfi99Rdfar_Bf0tXdbbtf7yx2n3gm0hLjti0yxX-W_U9ui7BoIhc0YAewByYFLhMbz6VU1fBAXKa4VFAwdra5TRXZ4S2jcCJPgiJ46dZwWoLAWWt008nxWnmTAtX0GUhb6haMwFIeqPu1PjNLXHMKMm1KmTp462pMVw519hsPPbpUYk9aPcjrg9jK0WFo6mOi2ScTJM5m0aQpBGbRZHg-3AxL6ZxWIh0Ek4ZPZ8DihP0qn9oFs6mo3mUzsM4iZNZmMZpQE8YXkxGaZiEi3gSR8kiTGJkvWqNNU9G83TWSfzs_ntj0Pn-1j-87v0F1Oi2rG4mS-P72KMNdhgMbpxyeFeaRue_qQ8zCQ?type=png)](https://mermaid.live/edit#pako:eNp1U02P2jAQ_SuWzykQEhKIKiREt1y2Eu2ylyoXY08Sa4Od2g4tu-K_d5zwkVW3OTkz7z3Pmxm_Ua4F0Ixa-NWC4vBFstKwQ64Ifg0zTnLZMOXIijBLvgKIPeMvZNU0_0LWG49ZoyDZgALDnDY9inE8kp1P7wyTmBvGtz6-vQv1ud2n5XK9ya5SQIrr5c8_Hu3nvRkvCyQPCrA9cb1B5i4jj9I6ogvSgLFasVq-guhpQ6H3nCfgBhzhujUWiO9Nn1caC9BHQBPBNkOPXd6xF7CkqRm_wGqtG_KAsNOwrrsfpG4v1byz0yO2iFghwuijFAO_rZWq_AAPStylkfi99Rdfar_Bf0tXdbbtf7yx2n3gm0hLjti0yxX-W_U9ui7BoIhc0YAewByYFLhMbz6VU1fBAXKa4VFAwdra5TRXZ4S2jcCJPgiJ46dZwWoLAWWt008nxWnmTAtX0GUhb6haMwFIeqPu1PjNLXHMKMm1KmTp462pMVw519hsPPbpUYk9aPcjrg9jK0WFo6mOi2ScTJM5m0aQpBGbRZHg-3AxL6ZxWIh0Ek4ZPZ8DihP0qn9oFs6mo3mUzsM4iZNZmMZpQE8YXkxGaZiEi3gSR8kiTGJkvWqNNU9G83TWSfzs_ntj0Pn-1j-87v0F1Oi2rG4mS-P72KMNdhgMbpxyeFeaRue_qQ8zCQ)

1. The trainer initiates the process by requesting the code generator to create personalized feedback URLs for each participant.
2. The code generator returns a list of unique feedback URLs. It also returns a secret course code that the trainer must remember.
3. For every participant:
   * The trainer sends out individual feedback links to each participant.
   * Each participant visits their personalized feedback link and submits their feedback using the app.
4. Once feedback is collected, the trainer queries the app to retrieve the responses. She must provide the secret course code to access the feedback.
5. The feedback app returns the submitted feedback to the trainer if the course code is valid.

### Code Generator

The code generator is a simple console app (.NET) that generates unique feedback URLs for each participant and prints them on the console. The trainer must provide the following information as commend-line arguments:

* The course code (mandatory, max. 20 characters)
* The course name (mandatory, max. 200 characters)
* Deadline for feedback submission (mandatory, date)
* The number of participants (>= 1, < 100, optional, default is 30)

The code generator will create a list of unique feedback URLs for each participant. The URLs should be in the following format: `https://someserver/feedback/{unique_feedback_code}`. _someserver_ might be _localhost:5000_ during development.

The _unique feedback code_ must be easy to read and remember. Therefore, the following rules apply:

* The code must be 8 characters long.
* The code must consist of six lowercase letters (a-z) followed by two digits (0-9).

The code generator must not generate the same feedback URL twice.

The code generator cannot regenerate feedback URLs for the same course code. If the trainer tries to generate feedback URLs for a course code that already exists, the app must display an error message and exit.

The code generator must also generate a random, secret course code that the trainer must remember. For the secret course code, the same rules apply as for the unique feedback code.

### Giving Feedback

The feedback app is a web application (Angular) that allows participants to submit their feedback. A participant starts the feedback process by clicking on the link sent by the trainer.

The app must display course code and course name.

The app must ask the participant for the following information:

* _Was the course helpful?_ (mandatory, 1..10, 1 = worst, 10 = best)
* _Have you been satisified with the course organization?_ (mandatory, 1..10, 1 = worst, 10 = best)
* _How knowledgeable was the trainer?_ (mandatory, 1..10, 1 = worst, 10 = best)
* _What did you like most about the course?_ (free text, optional, max. 500 characters)
* _What did you like least about the course?_ (free text, optional, max. 500 characters)

Once the participant has submitted the feedback, the app must display a thank you message.

Feedback links can only be used once. It is not possible to change the feedback after it has been submitted. It is not possible to provide feedback after the deadline. If a feedback URL is not usable (e.g. because already used, after deadline, etc.), a generic error message must be displayed because of security reasons. The app must not reveal any information about the error (e.g. "Feedback link is invalid" or "Feedback deadline has passed"). The app must not display the course code or course name in this case.

### Retrieving Feedback

Retrieving feedback is done in the same Angular web app as giving feedback.

The trainer must enter two pieces of information to retrieve the feedback:

* The course code
* The secret course code (generated by the code generator)

The app must display the following information:

* The course code
* The course name
* Indicator for whether the feedback process is still open or has been stopped
* Number of participants
* Number of feedbacks submitted
* The average rating for each question
* The feedback comments (if any) for each question

It is not possible to look at individual feedbacks.

After viewing the feedback, the trainer must be able to stop the feedback process. This means that no further feedback can be submitted (i.e. all feedback links are invalidated). However, the trainer can still view the feedback that has already been submitted.

If the feedback process has been stopped, the trainer must be able to delete the entire course. This means that course and feedback data are deleted from the database. The trainer must confirm the deletion by entering the course code again.

## Quality Criteria

* Use the latest version of Angular, .NET, and Entity Framework Core
* Choose any relational database you like (e.g. SQLite, PostgreSQL, SQL Server, etc.)
* Create at least the following projects:
  * Data access layer - all code related to database access
  * Code generator - console app for generating feedback URLs
  * Feedback API - ASP.NET Core Minimal API for giving and retrieving feedback
  * Feedback app - Angular web app for giving and retrieving feedback
* Write xUnit unit tests for the generation of feedback URLs
* The feedback app must be responsive and decently work on the following screen sizes:
  * Desktop with large monitor (Full HD or larger)
  * Phone in portrait mode and landscape mode
* Do not use any CSS frameworks - we want to practice our CSS skills

Optional extra features if you have time:

* Integrate a captcha to prevent spam
* Create Dockerfiles for the web API, web app, and code generator. Write a detailed readme file describing how to run the app based on the Docker images. Note: You can use Docker or Podman.
* Create a Docker Compose file to run the web API and the web app. Data must be stored outside containers on the host computer or in a Docker volume to ensure that data is not lost when the containers are stopped.
* Complex: Add end-to-end tests using Playwright.

## Process

Try to implement a _Minimal Viable Product_ (MVP) first. For that, reduce the scope of the project to the absolute minimum (e.g. ignore error handling, validations, and other non-functional requirements, simplify the project as much as possible). Focus on the core functionality first.

Once you have a working MVP, enhance your application in multiple iterations. Implement one feature at a time. Test your application after each step. This will help you to identify problems early and fix them before they become too complex.
