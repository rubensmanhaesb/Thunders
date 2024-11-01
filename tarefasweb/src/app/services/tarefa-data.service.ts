import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TarefaDataService {
  private tarefa: any; // Variável para armazenar o objeto tarefa

  // Método para definir o objeto tarefa
  setTarefa(tarefa: any): void {
    this.tarefa = tarefa;
  }

  // Método para obter o objeto tarefa
  getTarefa(): any {
    return this.tarefa;
  }
}
