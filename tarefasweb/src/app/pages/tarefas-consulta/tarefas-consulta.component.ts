import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TarefasCadastroComponent } from '../tarefas-cadastro/tarefas-cadastro.component';
import { UtilService } from '../../services/util.service';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';
import { TarefasEdicaoComponent } from "../tarefas-edicao/tarefas-edicao.component";


@Component({
  selector: 'app-tarefas-consulta',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TarefasCadastroComponent,
    RouterLink,
    TarefasEdicaoComponent
],
  templateUrl: './tarefas-consulta.component.html',
  styleUrl: './tarefas-consulta.component.css',
})
export class TarefasConsultaComponent implements OnInit {
  //variáveis
  tarefa: any = null;
  tarefas: any[] = [];
  tarefaSelecionada: any = null;
  mensagem: string = '';

  //método executado no momento em que
  //o componente é carregado
  ngOnInit(): void {
    // Detecta quando a navegação é concluída e chama buscarTarefas automaticamente
    this.buscarTarefas();

}
  //método construtor (injeção de dependência)
  constructor(private httpClient: HttpClient, private utilService: UtilService, private router: Router) {}
   
  //criando o formulário de pesquisa
  form = new FormGroup({
    search: new FormControl('')  // Controle de pesquisa vazio apenas para identificar o formulário
  });

  //função auxiliar para exibir os erros de validação
  get f(): any {
    return this.form.controls;
  }

  /*
    Função para capturar o SUBMIT do formulário
  */
  buscarTarefas(): void {
    console.log("buscarTarefas foi chamado");
      //executando a consulta de tarefas na API
      this.httpClient
        .get<any[]>(this.utilService.getTasksEndpoint())
        .subscribe({
          //capturando a resposta obtida da API
          next: (data) => {
            //armazenar os dados obtidos da consulta
            console.log('Dados recebidos:', data); // Inspeção dos dados recebidos
            
            this.tarefas = data.map(tarefa => {
              return {
                ...tarefa,
                data: tarefa.dataHora ? new Date(tarefa.dataHora) : null,
             
                prioridadeTarefa: this.getPrioridadeNome(tarefa.prioridade)
              };
            });
            
            if (this.tarefas.length > 0) {
              this.mensagem = 'Consulta realizada com sucesso.';
            } else {
              this.mensagem =
                'Nenhum registro foi encontrado para as datas selecionadas.';
            }
          },
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
  obterTarefa(id: string) : void {
    // Exibindo o spinner
  
    // Fazendo uma consulta na API para obter 1 tarefa através do ID
    this.httpClient.get<any>(`${this.utilService.getTasksEndpoint()}/${id}`)
      .subscribe({
        next: (tarefa) => { // Resposta de sucesso
          // Convertendo dataHora para um objeto Date
          this.tarefa = {
            ...tarefa,
            data: tarefa.dataHora ? new Date(tarefa.dataHora) : null,
            prioridadeTarefa: this.getPrioridadeNome(tarefa.prioridade)
          };
          console.log('Tarefa recebida:', this.tarefa); // Verificação da conversão
  
        },
        error: (error) => {
          console.error('Erro ao obter tarefa:', error);

        }
      });
  }
  
  alterarTarefa(tarefa: any): void {
    this.tarefaSelecionada = tarefa; // Define a tarefa para edição
  }

  excluirTarefa() : void {


    //fazendo uma requisição do tipo DELETE na API
    this.httpClient.delete(`${this.utilService.getTasksEndpoint()}/${this.tarefa.id}`)
      .subscribe({ //capturar o retorno da API
        next: (data: any) => { //resposta de sucesso..
          this.mensagem = `Tarefa '${data.nome}', excluída com sucesso.`;
          this.buscarTarefas(); //executando uma nova consulta na API
        
        }
      })
  }

}
