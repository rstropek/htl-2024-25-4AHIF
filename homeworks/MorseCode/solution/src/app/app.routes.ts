import { Routes } from '@angular/router';
import { EncodeComponent } from './encode/encode.component';
import { DecodeComponent } from './decode/decode.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/encode' },
  { path: 'encode', component: EncodeComponent },
  { path: 'decode', component: DecodeComponent },
];
