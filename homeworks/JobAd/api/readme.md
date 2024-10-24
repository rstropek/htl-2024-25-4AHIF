
# Todo List REST API

This project is a simple REST API that manages a list of todo items, built using **Node.js**, **Express**, **TypeScript**, and **SQLite**. The API allows you to create, read, update, and delete todo items, as well as filter them by title and assigned user. The todo items are stored in a SQLite database, and Swagger documentation is provided for easy API interaction.

## Features

- **CRUD Operations**: Supports creating, retrieving, updating, and deleting todo items.
- **Filters**: Ability to filter the todo list by title and assigned user.
- **SQLite**: Data is stored in a local SQLite database.
- **CORS Support**: API can be accessed from web browsers (CORS is enabled).
- **Swagger Documentation**: Interactive API documentation provided via Swagger UI.

## Getting Started

### Prerequisites

Make sure you have **Node.js** and **npm** installed on your machine. You can download them from [Node.js official website](https://nodejs.org/).

### Installation

1. Clone the repository or copy the source code to your local machine.

2. Install the required dependencies:

   ```bash
   npm install
   ```

### Build and Run

1. Compile the TypeScript code to JavaScript:

   ```bash
   npm run build
   ```

   This will generate the `app.js.js` file in the _dist_ directory.

2. Start the server:

   ```bash
   npm start
   ```

   The API server will run on `http://localhost:3000`.

3. Access the Swagger UI for API documentation and interactive testing:

   Open your browser and navigate to `http://localhost:3000/docs`.

## API Operations

### Endpoints

- **Create a Todo Item**:
  - **POST** `/todos`
- **List Todo Items**:
  - **GET** `/todos`
- **Get a Todo Item by ID**:
  - **GET** `/todos/{id}`
- **Update a Todo Item**:
  - **PATCH** `/todos/{id}`
- **Delete a Todo Item**:
  - **DELETE** `/todos/{id}`

### Data Structure

The todo item has the following fields:

- `id`: Auto-incremented integer (primary key).
- `title`: A string representing the title of the todo item.
- `assignedTo`: A string representing who the task is assigned to.
- `done`: A boolean indicating whether the todo item is completed.

## Accessing API Documentation

You can explore and interact with the API using the Swagger UI. Once the server is running, navigate to `http://localhost:3000/docs` in your web browser. Swagger provides a full list of available operations and allows you to test them directly from the browser.

## Database

**Note**: The database is persisted in a SQLite file named `todos.db`. The table `todos` is automatically created if it does not exist when the server starts.
