# TarefasApp

Configurações estão no appsettings.
Banco de Dados - rodar a migration 
O Arquivo NugetPackages.docx - contém as extensões utilizadas nos projetos em Angular e em .NET (Nuget)

## Requisitos

-  Angular
- .NET 8 SDK
- Visual Studio 2022 ou superior
- Docker (Opcional)
- SQL Server (ou outro banco de dados configurado)
- RabbitMQ
- MongoDB

- 
## Funcionalidades

- Gerenciamento de tarefas
- CRUD de tarefas 
- API RESTful para interação com as tarefas
- Implementação de princípios DDD e TDD para desenvolvimento orientado a testes


## Tecnologias Utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- XUnit (Framework de Testes)
- Domain-Driven Design (DDD)
- Test-Driven Development (TDD)
- MicroServiços
- RabbitMQ (Mensageria

## Principais Ferramentas e Bibliotecas

1.	.Angular:
2.	.NET 8:
3.	C# 12.0:
4.	ASP.NET Core:
5.	Middleware:
6.	Exceções Personalizadas:
7.	Serialização JSON:
8.	HTTP:
9.	RabbitMQ:
10.	MongoDB:
11.	Docker:
12.	Teste unitário
13.	Teste de integração

## Estrutura do Projeto

- TarefasWeb (Angular)
- TarefasApp.API (.NET)
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


