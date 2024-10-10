import { Routes } from '@angular/router';
import { PonyListComponent } from './pony-list/pony-list.component';

export const routes: Routes = [
  { path: 'pony-list', component: PonyListComponent },
  { path: '', redirectTo: '/pony-list', pathMatch: 'full' }
];
