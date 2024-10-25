# Angular Coding Exercise: Job Ads Management UI

![Hero image](hero.png)

## Introduction

Welcome to this Angular coding exercise! Your task is to build a web application that interacts with a provided [API](api) to manage job advertisements. The application should allow users to view, update, and manage job ads and their translations in multiple languages. The exercise is divided into basic and advanced requirements to guide your development process.

In this execise, we use another AI service: [DeepL](www.deepl.com). This service provides professional AI-based, professional translation services. You will get an API key from your teacher. **Note** that we have limited credits for this service. Therefore, do not translate long texts.

## Learning Goals

* Practice Angular fundamentals (components, services, routing, HTTP requests, etc.)
* Work with RESTful APIs
* Use a third-party artificial intelligence service (DeepL)

## Starter Code

The folder [ui](ui) contains the starter code for this exercise.

## UI Design

This exercise does not require a specific UI design. Coming up with a user-friendly design is part of the exercise. This specification only describes the functionality that must be implemented.

## Basic Requirements

*These requirements must be completed by everyone.*

1. List of All Job Ads

   - **Description:** Create a page that displays a list of all job advertisements.
   - **API Endpoint:** `GET /ads` - Retrieves all job ads.
   
2. Delete a Job Ad via a Link

   - **Description:** Allow users to delete a job ad directly from the list using a link or button.
   - **API Endpoint:** `DELETE /ads/{id}` - Deletes a job ad by its ID.

3. Link to Job Ad Detail Page

   - **Description:** Each job ad in the list should link to its detail page.
   - **Navigation:** Implement routing to navigate to the detail page of a selected job ad.

4. Job Ad Detail Page Displaying Data and Translations

   - **Description:** On the job ad detail page, display all the job ad data, including all available translations.
   - **API Endpoint:** `GET /ads/{id}` - Retrieves a specific job ad by ID, including its translations.

5. Update Title and EN Text of a Job Ad

   - **Description:** Allow users to update the title and English text (`textEN`) of a job ad from its detail page.
   - **API Endpoint:** `PATCH /ads/{id}` - Updates the title and/or `textEN` of a job ad.

## Advanced Requirements

*These requirements are for those who want to go beyond the basics.*

1. Clean and Nice Design

   - **Description:** The application should have a clean and user-friendly design. It doesn't need to be elaborate but should be visually appealing.

2. Add Translations to Additional Languages

   - **Description:** Enable users to add translations for additional languages on the job ad detail page.
   - **API Endpoint:** `PUT /ads/{id}/translations/{language}` - Upserts a translation for a job ad.

3. Delete Translations of a Job Ad

   - **Description:** Allow users to delete translations of a job ad.
   - **API Endpoint:** `DELETE /ads/{id}/translations/{language}` - Deletes a translation for a specific language.

4. Navigate Back to the List of Job Ads

   - **Description:** Provide a way for users to navigate back to the list of job ads from the detail page.

5. Auto-Translate Feature

   - **Description:** When adding a translation, include an "Auto-Translate" button that uses the translation API to generate a translated text from English into the selected language.
   - **API Endpoint:** `POST /deepl/v2/translate` - Translates text using the DeepL API.

## Tips

- **Starting the API on your computer:**

  - Navigate to the [api](api) folder.
  - Run `npm install` to install all dependencies.
  - Run `npm start` to start the API server.

- **API Documentation:**

  - Access the OpenAPI Specification (Swagger UI) at [http://localhost:3000/swagger](http://localhost:3000/swagger).
  - Use this interface to explore the API endpoints and test requests.

- **Sample API Requests:**

  - Refer to the [requests.http](api/requests.http) file for sample API requests (you need [REST Client extension in VSCode](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) to use it).
  - These examples can help you understand how to interact with the API.

**Happy coding!**
