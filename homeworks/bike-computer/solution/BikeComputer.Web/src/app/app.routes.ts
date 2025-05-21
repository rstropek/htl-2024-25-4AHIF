import { Routes } from '@angular/router';
import { BikeListComponent } from './bike-list/bike-list.component';
import { RegisterBikeComponent } from './register-bike/register-bike.component';

export const routes: Routes = [
  { path: '', redirectTo: 'bike-list', pathMatch: 'full' },
  { path: 'bike-list', component: BikeListComponent },
  { path: 'register-bike', component: RegisterBikeComponent },
];
