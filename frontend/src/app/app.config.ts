import {
  ApplicationConfig,
  APP_INITIALIZER,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
  importProvidersFrom
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { routes } from './app.routes';
import { NgChartsModule } from 'ng2-charts';

import { ContactService } from './services/contact.service';

function preloadPublicInfoFactory(contact: ContactService) {
  return () => contact.preloadPublicInfo();
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    importProvidersFrom(NgChartsModule),
    provideHttpClient(withInterceptors([AuthInterceptor])),

    {
      provide: APP_INITIALIZER,
      useFactory: preloadPublicInfoFactory,
      deps: [ContactService],
      multi: true
    }
  ]
};
