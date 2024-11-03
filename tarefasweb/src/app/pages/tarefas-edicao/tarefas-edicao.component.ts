

import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, Input, OnChanges, SimpleChanges} from '@angular/core';
import { UtilService } from '../../services/util.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TarefaDataService } from '../../services/tarefa-data.service';

@Component({
  selector: 'app-tarefas-edicao',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './tarefas-edicao.component.html',
  styleUrl: './tarefas-edicao.component.html'
})
export class TarefasEdicaoComponent implements OnInit, OnChanges {
  @Input() tarefa: any;
  mensagem: string = '';
  
  ngOnInit(): void {
    // Garantir que a tarefa está definida ao iniciar o componente
    if (this.tarefa) {
      this.preencherFormulario(this.tarefa);
    }
  }

  // Detecta mudanças na tarefa passada como @Input
  ngOnChanges(changes: SimpleChanges): void {
    //console.log('ngOnChanges - ', this.tarefa);
    this.mensagem = ''; // Limpa a mensagem de erro
    if (changes['tarefa'] && changes['tarefa'].currentValue) {
      this.preencherFormulario(changes['tarefa'].currentValue);
    }
  }

  // Função para preencher o formulário com os valores da tarefa
  private preencherFormulario(tarefa: any): void {
    // Converte o timestamp ISO para um objeto Date
    const dataHora = tarefa.dataHora ? new Date(tarefa.dataHora) : null;
    const dataFormatada = dataHora ? dataHora.toISOString().split('T')[0] : ''; // Formato yyyy-MM-dd
    const horaFormatada = dataHora ? dataHora.toISOString().split('T')[1].slice(0, 5) : ''; // Formato HH:mm
    //

    this.form.patchValue({
      id: tarefa.id, // Atribui o ID aqui
      nome: tarefa.nome,
      data: dataFormatada,
      hora: horaFormatada,
      descricao: tarefa.descricao,
      prioridade: tarefa.prioridade
    });
  }


  constructor(
      private httpClient: HttpClient, private utilService: UtilService,  
      private router: Router,  private tarefaDataService: TarefaDataService) {  }

  //criando a estrutura do formulário
  form = new FormGroup({
    id: new FormControl(''), // Campo para armazenar o ID
    nome : new FormControl('', [Validators.required]),
    data: new FormControl('', [Validators.required]),
    hora: new FormControl('', [Validators.required]),
    descricao : new FormControl('', [Validators.required]),
    prioridade : new FormControl('', [Validators.required])
  });

  //função para verificar as validações dos campos
  get f() : any {
    return this.form.controls;
  }

  alterarTarefa() {
    const id = this.form.value.id; // Obtém o ID do formulário

    this.httpClient.put(this.utilService.getTasksEndpoint(), this.form.value)
      .subscribe({
        next: (dados: any) => {
          // Exibindo mensagem de sucesso
          this.mensagem = `Tarefa alterada com sucesso. ID: ${dados.id}`;
          // Limpando os campos do formulário
          this.form.reset();          
        },
        error: (error) => {
          console.error('Erro ao alterar tarefa:', error); // Loga o erro para depuração
  
          // Exibir mensagem de erro
          if (error.status === 400 && error.error?.errors) {
            // Se for um erro de validação (400), exiba as mensagens de erro
            this.mensagem = 'Erro ao alterar tarefa: ' + Object.values(error.error.errors).join(', ');
            console.log('dentro do 400 Erro de validação:' + Object.values(error.error.errors).join(', '));
          } else {
            // Outros erros
            console.log('fora do 400 Erro de validação:');
            this.mensagem = 'Erro ao alterar tarefa. Por favor, tente novamente.';
          }
  
        }
      });
    }
  
}
