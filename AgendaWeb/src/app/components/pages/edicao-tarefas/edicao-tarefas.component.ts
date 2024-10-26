import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-edicao-tarefas',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './edicao-tarefas.component.html',
  styleUrl: './edicao-tarefas.component.css'
})
export class EdicaoTarefasComponent {

  //variáveis
  mensagem: string = '';

  //método construtor
  constructor(
    private httpClient: HttpClient,
    private spinner: NgxSpinnerService
  ) {}

  //objeto para capturar os campos do formulário
  formulario = new FormGroup({
    nome: new FormControl('', [Validators.required, Validators.minLength(2)]),
    data: new FormControl('', [Validators.required]),
    hora: new FormControl('', [Validators.required]),
    descricao: new FormControl('', [Validators.required, Validators.minLength(2)]),
    prioridade: new FormControl('', [Validators.required])
  });

  get f() {
    return this.formulario.controls;
  }

  editarTarefa() {
    // Exibir o spinner
    this.spinner.show();
  
    this.httpClient.put('http://localhost:5267/api/tarefas', this.formulario.value)
      .subscribe({
        next: (dados: any) => {
          // Exibindo mensagem de sucesso
          this.mensagem = `Tarefa editada com sucesso. ID: ${dados.id}`;
          // Limpando os campos do formulário
          this.formulario.reset();
          // Fechando o spinner
          this.spinner.hide();
        },
        error: (error) => {
          console.error('Erro ao editar tarefa:', error); // Loga o erro para depuração
  
          // Exibir mensagem de erro
          if (error.status === 400 && error.error?.errors) {
            // Se for um erro de validação (400), exiba as mensagens de erro
            this.mensagem = 'Erro ao editar tarefa: ' + Object.values(error.error.errors).join(', ');
          } else {
            // Outros erros
            this.mensagem = 'Erro ao editar tarefa. Por favor, tente novamente.';
          }
  
          // Fechar o spinner
          this.spinner.hide();
        }
      });
  }
}  