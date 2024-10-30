import { Routes } from '@angular/router';
import { TarefasCadastroComponent } from './pages/tarefas-cadastro/tarefas-cadastro.component';
import { TarefasConsultaComponent } from './pages/tarefas-consulta/tarefas-consulta.component';
import { TarefasEdicaoComponent } from './pages/tarefas-edicao/tarefas-edicao.component';

export const routes: Routes = [
    {
        path: '', redirectTo: '/pages/tarefa-consulta',
        pathMatch: 'full',
    },
    {
        path: 'pages/tarefa-cadastro',
        component: TarefasCadastroComponent
    },
    {
        path: 'pages/tarefa-consulta',
        component: TarefasConsultaComponent
    },
    {
        path: 'pages/tarefa-edicao/:id',
        component: TarefasEdicaoComponent
    }
];
