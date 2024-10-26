using AgendaApp.Tests.Helpers;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Dtos;
using Xunit;

namespace AgendaApp.Tests
{

    public class TarefasTest
    {
        [Fact]
        public void CadastrarTarefa_Test()
        {
            var request = CreateRequest(); 
            var response = CadastrarTarefa(request); 

            response?.Id.Should().NotBeEmpty();
            response?.Nome.Should().Be(request.Nome);
            response?.DataHora.Should().Be(request.DataHora);
            response?.Prioridade?.Should().Be(request.Prioridade);
        }

        [Fact]
        public void EditarTarefa_Test()
        {
            var request = CreateRequest(); 
            var tarefa = CadastrarTarefa(request); 

            var result = TestHelper.CreateClient().PutAsync("/api/tarefas/" + tarefa.Id, TestHelper.CreateContent(request)).Result;
            var response = TestHelper.ReadResponse<TarefaDto>(result);

            response?.Id.Should().Be(tarefa.Id);
            response?.Nome.Should().Be(tarefa.Nome);
            response?.DataHora.Should().Be(tarefa.DataHora);
            response?.Prioridade?.Should().Be(tarefa.Prioridade);
        }

        [Fact]
        public void ExcluirTarefa_Test()
        {
            var request = CreateRequest(); 
            var tarefa = CadastrarTarefa(request); 

            var result = TestHelper.CreateClient().DeleteAsync("/api/tarefas/" + tarefa.Id).Result;
            var response = TestHelper.ReadResponse<TarefaDto>(result);

            response?.Id.Should().Be(tarefa.Id);
            response?.Nome.Should().Be(tarefa.Nome);
            response?.DataHora.Should().Be(tarefa.DataHora);
            response?.Prioridade?.Should().Be(tarefa.Prioridade);
        }

        [Fact]
        public void ConsultarTarefasPorDatas_Test()
        {
            var request = CreateRequest(); 
            var tarefa = CadastrarTarefa(request); 

            var dataMin = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            var dataMax = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            var result = TestHelper.CreateClient().GetAsync("/api/tarefas/" + dataMin + "/" + dataMax).Result;
            var response = TestHelper.ReadResponse<List<TarefaDto>>(result);

            var cadastro = response.FirstOrDefault(t => t.Id == tarefa.Id);

            cadastro?.Nome.Should().Be(tarefa.Nome);
            cadastro?.DataHora.Should().Be(tarefa.DataHora);
            cadastro?.Prioridade?.Should().Be(tarefa.Prioridade);
        }

        [Fact]
        public void ObterTarefaPorId_Test()
        {
            var request = CreateRequest(); 
            var tarefa = CadastrarTarefa(request); 

            var result = TestHelper.CreateClient().GetAsync("/api/tarefas/" + tarefa.Id).Result;
            var response = TestHelper.ReadResponse<TarefaDto>(result);

            response?.Nome.Should().Be(tarefa.Nome);
            response?.DataHora.Should().Be(tarefa.DataHora);
            response?.Prioridade?.Should().Be(tarefa.Prioridade);
        }

        private TarefaDto CreateRequest()
        {
            var faker = new Faker("pt_BR");
            var request = new TarefaDto
            {
                Nome = faker.Lorem.Sentences(1),
                DataHora = DateTime.Now,
                Prioridade = (Prioridade?)faker.Random.Int(1, 3)
            }; 

            return request;
        }

        private TarefaDto CadastrarTarefa(TarefaDto request)
        {
            var result = TestHelper.CreateClient().PostAsync("/api/tarefas", TestHelper.CreateContent(request)).Result;
            return TestHelper.ReadResponse<TarefaDto>(result);
        }
    }
}
