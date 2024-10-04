import { ApplicationConfig, InjectionToken, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { morseCodeAlphabet } from './alphabet';

export const ALPHABET = new InjectionToken<string[]>('Alphabet');

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    { provide: ALPHABET, useValue: morseCodeAlphabet }
  ]
};
