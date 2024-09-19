import { Routes } from '@angular/router';
import { TicTacToeComponent } from './tic-tac-toe/tic-tac-toe.component';
import { GreetingComponent } from './greeting/greeting.component';
import { NumberGuessingComponent } from './number-guessing/number-guessing.component';
import { TodoListComponent } from './todos/todo-list/todo-list.component';

export const routes: Routes = [
    { path: 'tic-tac-toe', component: TicTacToeComponent },
    { path: 'number-guessing', component: NumberGuessingComponent },
    { path: 'todo-list', component: TodoListComponent },
    { path: '', pathMatch: 'full', component: GreetingComponent }
];
