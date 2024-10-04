import express from 'express';
import cors from 'cors';
import sqlite3 from 'sqlite3';
import swaggerUi from 'swagger-ui-express';
import swaggerJsdoc from 'swagger-jsdoc';

const app = express();
const port = 3000;

app.use(cors());
app.use(express.json());

// Initialize the SQLite database
const db = new sqlite3.Database('./todos.db', (err) => {
    if (err) {
        console.error('Could not connect to SQLite database', err);
    } else {
        console.log('Connected to SQLite database');
    }
});

db.run(`CREATE TABLE IF NOT EXISTS todos (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    assignedTo TEXT NOT NULL,
    done INTEGER NOT NULL DEFAULT 0
)`);

// Swagger setup
const options = {
    definition: {
        openapi: '3.0.0',
        info: {
            title: 'Todo API',
            version: '1.0.0',
        },
        components: {
            schemas: {
                Todo: {
                    type: 'object',
                    properties: {
                        id: {
                            type: 'integer',
                        },
                        title: {
                            type: 'string',
                        },
                        assignedTo: {
                            type: 'string',
                        },
                        done: {
                            type: 'boolean',
                        },
                    },
                },
            },
        },
    },
    apis: ['./src/app.ts'], // Adjust the filename if necessary
};

const specs = swaggerJsdoc(options);
app.use('/docs', swaggerUi.serve, swaggerUi.setup(specs));

/**
 * @swagger
 * tags:
 *   - name: Todos
 *     description: API for managing todo items
 */

/**
 * @swagger
 * /todos:
 *   post:
 *     summary: Create a new todo item
 *     tags:
 *       - Todos
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             required:
 *               - title
 *               - assignedTo
 *             properties:
 *               title:
 *                 type: string
 *               assignedTo:
 *                 type: string
 *     responses:
 *       201:
 *         description: Todo item created successfully
 *         content:
 *           application/json:
 *             schema:
 *               $ref: '#/components/schemas/Todo'
 */
app.post('/todos', (req, res) => {
    const { title, assignedTo } = req.body;
    if (!title || !assignedTo) {
        return res.status(400).json({ error: 'Title and AssignedTo are required' });
    }
    const done = 0;
    db.run(
        'INSERT INTO todos (title, assignedTo, done) VALUES (?, ?, ?)',
        [title, assignedTo, done],
        function (err) {
            if (err) {
                return res.status(500).json({ error: err.message });
            }
            const id = this.lastID;
            res.status(201).json({ id, title, assignedTo, done: false });
        }
    );
});

/**
 * @swagger
 * /todos:
 *   get:
 *     summary: List all todo items
 *     tags:
 *       - Todos
 *     parameters:
 *       - in: query
 *         name: title
 *         schema:
 *           type: string
 *         required: false
 *         description: Filter by title
 *       - in: query
 *         name: assignedTo
 *         schema:
 *           type: string
 *         required: false
 *         description: Filter by assignedTo
 *     responses:
 *       200:
 *         description: A list of todo items
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 $ref: '#/components/schemas/Todo'
 */
app.get('/todos', (req, res) => {
    let sql = 'SELECT * FROM todos WHERE 1=1';
    const params: any[] = [];

    if (req.query.title) {
        sql += ' AND title LIKE ?';
        params.push(`%${req.query.title}%`);
    }
    if (req.query.assignedTo) {
        sql += ' AND assignedTo LIKE ?';
        params.push(`%${req.query.assignedTo}%`);
    }

    db.all(sql, params, (err, rows) => {
        if (err) {
            return res.status(500).json({ error: err.message });
        }
        // Convert 'done' from integer to boolean
        const todos = rows.map((row: any) => ({
            id: row.id,
            title: row.title,
            assignedTo: row.assignedTo,
            done: !!row.done,
        }));
        res.json(todos);
    });
});

/**
 * @swagger
 * /todos/{id}:
 *   get:
 *     summary: Get a todo item by ID
 *     tags:
 *       - Todos
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID of the todo item
 *         schema:
 *           type: integer
 *     responses:
 *       200:
 *         description: A todo item
 *         content:
 *           application/json:
 *             schema:
 *               $ref: '#/components/schemas/Todo'
 *       404:
 *         description: Todo item not found
 */
app.get('/todos/:id', (req, res) => {
    const id = req.params.id;
    db.get('SELECT * FROM todos WHERE id = ?', [id], (err, row: any) => {
        if (err) {
            return res.status(500).json({ error: err.message });
        }
        if (!row) {
            return res.status(404).json({ error: 'Todo item not found' });
        }
        // Convert 'done' from integer to boolean
        const todo = {
            id: row.id,
            title: row.title,
            assignedTo: row.assignedTo,
            done: !!row.done,
        };
        res.json(todo);
    });
});

/**
 * @swagger
 * /todos/{id}:
 *   patch:
 *     summary: Update a todo item
 *     tags:
 *       - Todos
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID of the todo item
 *         schema:
 *           type: integer
 *     requestBody:
 *       description: Fields to update
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               title:
 *                 type: string
 *               assignedTo:
 *                 type: string
 *               done:
 *                 type: boolean
 *     responses:
 *       200:
 *         description: Todo item updated successfully
 *         content:
 *           application/json:
 *             schema:
 *               $ref: '#/components/schemas/Todo'
 *       404:
 *         description: Todo item not found
 */
app.patch('/todos/:id', (req, res) => {
    const id = req.params.id;
    const { title, assignedTo, done } = req.body;

    db.get('SELECT * FROM todos WHERE id = ?', [id], (err, row: any) => {
        if (err) {
            return res.status(500).json({ error: err.message });
        }
        if (!row) {
            return res.status(404).json({ error: 'Todo item not found' });
        }

        const updatedTitle = title !== undefined ? title : row.title;
        const updatedAssignedTo = assignedTo !== undefined ? assignedTo : row.assignedTo;
        const updatedDone = done !== undefined ? (done ? 1 : 0) : row.done;

        db.run(
            'UPDATE todos SET title = ?, assignedTo = ?, done = ? WHERE id = ?',
            [updatedTitle, updatedAssignedTo, updatedDone, id],
            function (err) {
                if (err) {
                    return res.status(500).json({ error: err.message });
                }
                res.json({
                    id,
                    title: updatedTitle,
                    assignedTo: updatedAssignedTo,
                    done: !!updatedDone,
                });
            }
        );
    });
});

/**
 * @swagger
 * /todos/{id}:
 *   delete:
 *     summary: Delete a todo item
 *     tags:
 *       - Todos
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID of the todo item
 *         schema:
 *           type: integer
 *     responses:
 *       204:
 *         description: Todo item deleted successfully
 *       404:
 *         description: Todo item not found
 */
app.delete('/todos/:id', (req, res) => {
    const id = req.params.id;
    db.run('DELETE FROM todos WHERE id = ?', [id], function (err) {
        if (err) {
            return res.status(500).json({ error: err.message });
        }
        if (this.changes === 0) {
            return res.status(404).json({ error: 'Todo item not found' });
        }
        res.status(204).send();
    });
});

app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});
