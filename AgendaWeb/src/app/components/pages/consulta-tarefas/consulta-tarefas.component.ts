import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-consulta-tarefas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxPaginationModule
  ],
  templateUrl: './consulta-tarefas.component.html',
  styleUrl: './consulta-tarefas.component.css'
})
export class ConsultaTarefasComponent {

  //variáveis
  tarefas: any[] = [];
  tarefa: any = null;

  mensagem: string = '';
  pagina: number = 1;

  //método construtor
  constructor(
    private httpClient: HttpClient,
    private spinner: NgxSpinnerService
  ) {}

  //objeto de formulário
  formulario = new FormGroup({
    dataMin: new FormControl('', [Validators.required]),
    dataMax: new FormControl('', [Validators.required])
  });

  //função auxiliar para verificar se os campos estão com erro
  //de preenchimento e exibir as mensagens de validação
  get f() {
    return this.formulario.controls;
  }

  pesquisarTarefas(): void {
    this.spinner.show();
  
    this.httpClient.get<any[]>(`http://localhost:5267/api/tarefas/`)
      .subscribe({
        next: (data) => {
          //console.log('Dados recebidos:', data); // Inspeção dos dados recebidos
  
          // Mapeamento dos dados com a prioridade baseada no valor numérico
          this.tarefas = data.map(tarefa => {
            //console.log('Data e Prioridade recebidas:', tarefa.dataHora, tarefa.prioridade); // Inspeciona dataHora e prioridade
  
            return {
              ...tarefa,
              data: tarefa.dataHora ? new Date(tarefa.dataHora) : null,

              prioridadeTarefa: this.getPrioridadeNome(tarefa.prioridade)
            };
          });
  
          this.spinner.hide();
        },
        error: (error) => {
          console.error('Erro ao carregar tarefas:', error);
          this.spinner.hide();
        }
      });
  }
  

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
  

  // Função para consultar os dados da tarefa através do ID
obterTarefa(id: string): void {
  // Exibindo o spinner
  this.spinner.show();

  // Fazendo uma consulta na API para obter 1 tarefa através do ID
  this.httpClient.get<any>(`http://localhost:5267/api/tarefas/${id}`)
    .subscribe({
      next: (tarefa) => { // Resposta de sucesso
        // Convertendo dataHora para um objeto Date
        this.tarefa = {
          ...tarefa,
          data: tarefa.dataHora ? new Date(tarefa.dataHora) : null,
          prioridadeTarefa: this.getPrioridadeNome(tarefa.prioridade)
        };
        console.log('Tarefa recebida:', this.tarefa); // Verificação da conversão

        // Fechando o spinner
        this.spinner.hide();
      },
      error: (error) => {
        console.error('Erro ao obter tarefa:', error);
        this.spinner.hide();
      }
    });
}


  //função para excluir a tarefa baseado no ID
  excluirTarefa() : void {

    //exibindo o componente spinner..
    this.spinner.show();

    //fazendo uma requisição do tipo DELETE na API
    this.httpClient.delete(`http://localhost:5267/api/tarefas/${this.tarefa.id}`)
      .subscribe({ //capturar o retorno da API
        next: (data: any) => { //resposta de sucesso..
          this.mensagem = `Tarefa '${data.nome}', excluída com sucesso.`;
          this.pesquisarTarefas(); //executando uma nova consulta na API
        
          //ocultar o spinner
          this.spinner.hide();
        }
      })
  }

  //função para trocar de página no componente de paginação
  handlePageChange(event: any): void {
    this.pagina = event;
  }

}
