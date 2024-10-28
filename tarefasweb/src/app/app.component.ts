import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from './layout/menu/menu.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    MenuComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  
  //atributos
  isAuthenticated: boolean = false;
  nomeUsuario: string = '';
  emailUsuario: string = '';

  ngOnInit(): void {

    //verificar se o usuário está autenticado
    if(localStorage.getItem('auth') != null) {

      //ler os dados do usuário contido na local storage
      const usuario = JSON.parse(localStorage.getItem('auth') as string);
      this.isAuthenticated = true;
      this.nomeUsuario = usuario.nome;
      this.emailUsuario = usuario.email;
    }
  }

  logout(): void {
    if(confirm('Deseja realmente sair do sistema?')) {
      //apagar os dados gravados na local storage
      localStorage.removeItem('auth');
      //redirecionar para a página de login
      location.href = '/pages/autenticar-usuario';
    }
  }

}
