// src/app/services/util.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UtilService {
  // Defina a URL base da API e os endpoints específicos
  readonly API_BASE_URL = 'http://localhost:8080/api';
  readonly TASKS_ENDPOINT = `${this.API_BASE_URL}/tarefas`;

  constructor() {}

  // Método para obter o endpoint completo de tarefas
  getTasksEndpoint(): string {
    return this.TASKS_ENDPOINT;
  }
}


