# TarefasApp

## Requisitos

- .NET 8 SDK
- Visual Studio 2022 ou superior
- Docker (Opcional)
- SQL Server (ou outro banco de dados configurado)
- RabbitMQ
- MongoDB

- 
## Funcionalidades

- Gerenciamento de tarefas
- CRUD de tarefas (Criar, Ler, Atualizar, Deletar)
- API RESTful para interação com as tarefas
- Implementação de princípios DDD e TDD para desenvolvimento orientado a testes


## Tecnologias Utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- XUnit (Framework de Testes)
- Domain-Driven Design (DDD)
- Test-Driven Development (TDD)

## Principais Ferramentas e Bibliotecas

1.	.NET 8:
2.	C# 12.0:
3.	ASP.NET Core:
4.	Middleware:
5.	Exceções Personalizadas:
6.	Serialização JSON:
7.	HTTP:
8.	RabbitMQ:
9.	MongoDB:
10.	Docker:
11.	Teste unitário
12.	Teste de integração

## Estrutura do Projeto


- TarefasApp.API
- Microsoft.AspNetCore.App: Inclui todos os pacotes necessários para construir uma aplicação ASP.NET Core.
- System.Text.Json: Para serialização e desserialização de JSON.
- Microsoft.Extensions.Logging: Para logging dentro da aplicação.
- Microsoft.AspNetCore.Http: Para manipulação de requisições e respostas HTTP.
- TarefasApp.Domain
- System.ComponentModel.Annotations: Para validações e anotações de dados.
- FluentValidation: Para validações de regras de negócio e entidade.
- TarefasApp.Infrastructure
- MongoDB.Driver: Para interação com o MongoDB.
- RabbitMQ.Client: Para interação com o RabbitMQ.
- TarefasApp.Tests
- xUnit: Framework de testes unitários.
- Moq: Para criação de mocks em testes.
- Microsoft.AspNetCore.Mvc.Testing: Para testes de integração em aplicações ASP.NET Core.


