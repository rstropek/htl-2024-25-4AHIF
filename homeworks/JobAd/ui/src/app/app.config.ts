import { ApplicationConfig, InjectionToken } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch } from '@angular/common/http';

export const BASE_URL = new InjectionToken<string>('BaseUrl');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    { provide: BASE_URL, useValue: 'http://localhost:3000' },
    provideHttpClient(withFetch())
  ]
};
