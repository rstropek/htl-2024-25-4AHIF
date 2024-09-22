import { Routes } from '@angular/router';
import { TicTacToeComponent } from './tic-tac-toe/tic-tac-toe.component';
import { GreetingComponent } from './greeting/greeting.component';
import { NumberGuessingComponent } from './number-guessing/number-guessing.component';
import { TodoListComponent as SinglePageTodoListComponent } from './todos/todo-list/todo-list.component';
import { TodosListComponent as MultiPageTodoListComponent } from './todos-simple/todos-list/todos-list.component';
import { TodosEditComponent } from './todos-simple/todos-edit/todos-edit.component';

export const routes: Routes = [
    { path: 'tic-tac-toe', component: TicTacToeComponent },
    { path: 'number-guessing', component: NumberGuessingComponent },
    { path: 'todo-list', component: SinglePageTodoListComponent },
    { path: 'todo-list-simple', component: MultiPageTodoListComponent },
    { path: 'todo-list-simple/edit', component: TodosEditComponent },
    { path: 'todo-list-simple/edit/:id', component: TodosEditComponent },
    { path: '', pathMatch: 'full', component: GreetingComponent }
];
