import { Routes } from '@angular/router';
import { TicTacToeComponent } from './tic-tac-toe/tic-tac-toe.component';
import { GreetingComponent } from './greeting/greeting.component';

export const routes: Routes = [
    { path: 'tic-tac-toe', component: TicTacToeComponent },
    { path: '', pathMatch: 'full', component: GreetingComponent }
];
