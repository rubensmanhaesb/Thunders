// src/app/services/prioridade.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PrioridadeService {
  constructor() {}

  getPrioridadeNome(prioridade: number): { nome: string } {
    switch (prioridade) {
      case 1:
        return { nome: 'Baixa' };
      case 2:
        return { nome: 'Média' };
      case 3:
        return { nome: 'Alta' };
      default:
        return { nome: 'Sem Prioridade' };
    }
  }

  getAllPrioridades(): { id: number; nome: string }[] {
    return [
      { id: 1, nome: 'Baixa' },
      { id: 2, nome: 'Média' },
      { id: 3, nome: 'Alta' },
    ];
  }

}
