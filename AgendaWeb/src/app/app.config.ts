import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), //habilitar a biblioteca de rotas
    provideHttpClient(), //habilitar a biblioteca de requisições HTTP
    provideAnimations() //habilitar bibliotecas de efeitos de animação
  ]
};
