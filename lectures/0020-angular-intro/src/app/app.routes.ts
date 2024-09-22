import { Routes } from '@angular/router';
import { TicTacToeComponent } from './tic-tac-toe/tic-tac-toe.component';
import { GreetingComponent } from './greeting/greeting.component';
import { NumberGuessingComponent } from './number-guessing/number-guessing.component';
import { TodoListComponent as SinglePageTodoList } from './todos/todo-list/todo-list.component';
import { TodosListComponent as TodoListView } from './todos-simple/todos-list/todos-list.component';
import { TodosEditComponent } from './todos-simple/todos-edit/todos-edit.component';

export const routes: Routes = [
    { path: 'tic-tac-toe', component: TicTacToeComponent },
    { path: 'number-guessing', component: NumberGuessingComponent },
    { path: 'todo-list', component: SinglePageTodoList },
    { path: 'todo-list2', component: TodoListView },
    { path: 'todo-list2/edit', component: TodosEditComponent },
    { path: 'todo-list2/edit/:id', component: TodosEditComponent },
    { path: '', pathMatch: 'full', component: GreetingComponent }
];
