import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UtilService } from '../../services/util.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-tarefas-cadastro',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './tarefas-cadastro.component.html',
  styleUrl: './tarefas-cadastro.component.css'
})
export class TarefasCadastroComponent implements OnInit {

  mensagem: string = '';
  
  constructor(private httpClient: HttpClient, private utilService: UtilService) {  }

  ngOnInit(): void {

  }

  //criando a estrutura do formulário
  form = new FormGroup({
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
  cadastrarTarefa() {
  
    this.httpClient.post(this.utilService.getTasksEndpoint(), this.form.value)
      .subscribe({
        next: (dados: any) => {
          // Exibindo mensagem de sucesso
          this.mensagem = `Tarefa cadastrada com sucesso. ID: ${dados.id}`;
          // Limpando os campos do formulário
          this.form.reset();
          
        },
        error: (error) => {
          console.error('Erro ao cadastrar tarefa:', error); // Loga o erro para depuração
  
          // Exibir mensagem de erro
          if (error.status === 400 && error.error?.errors) {
            // Se for um erro de validação (400), exiba as mensagens de erro
            this.mensagem = 'Erro ao cadastrar tarefa: ' + Object.values(error.error.errors).join(', ');
            console.log('Erro ao cadastrar tarefa:', error.error.errors);
          } else {
            // Outros erros
            this.mensagem = 'Erro ao cadastrar tarefa. Por favor, tente novamente.';
          }
  
        }
      });
    }



}
