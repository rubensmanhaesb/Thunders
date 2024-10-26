using Xunit;
using Bogus;
using TarefasApp.Domain.Entities;
using TarefasApp.Infra.Data.Contexts;
using TarefasApp.Infra.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace TarefasApp.Infra.Data.Test
{
    [TestClass]
    public class TarefasRepositoryTest
    {
        //atributos
        private readonly Faker<Tarefa> _fakerTarefa;
        private readonly DataContext _dataContext;
        private readonly TarefaRepository _tarefaRepository;
        public TarefasRepositoryTest()
        {
            _fakerTarefa = new Faker<Tarefa>("pt_BR")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                .RuleFor(c => c.DataHora, f => f.Date.Future())
                .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4));

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TarefasAppTestsDB")
                .Options;

            _dataContext = new DataContext(options);

            _tarefaRepository = new TarefaRepository(_dataContext);

        }

        [Fact(DisplayName = "Adicionar tarefas com sucesso no repositório.")]
        public async Task ShouldAddTarefa()
        {
            var tarefa = _fakerTarefa.Generate();

            await _tarefaRepository.Add(tarefa);

            var tarefaCadastrada = await _tarefaRepository.GetById((Guid)tarefa.Id);

            if (tarefaCadastrada == null)
                Xunit.Assert.True(false, "Tarefa não encontrada.");

            tarefaCadastrada?.Id.Should().Be(tarefa.Id);
            tarefaCadastrada?.Nome.Should().Be(tarefa.Nome);
            tarefaCadastrada?.Descricao.Should().Be(tarefa.Descricao);
            tarefaCadastrada?.DataHora.Should().Be(tarefa.DataHora);
            tarefaCadastrada?.Prioridade.Should().Be(tarefa.Prioridade);
        }

        [Fact(DisplayName = "Atualizar tarefa com sucesso no repositório.")]
        public async Task ShouldUpdateTarefa()
        {
            var tarefa = _fakerTarefa.Generate();

            await _tarefaRepository.Add(tarefa);

            tarefa.Nome = "Novo Nome";
            tarefa.Descricao = "Nova Descrição";
            tarefa.Prioridade = new Random().Next(1, 4); 

            await _tarefaRepository.Update(tarefa);

            var tarefaAtualizada = await _tarefaRepository.GetById((Guid)tarefa.Id);

            if (tarefaAtualizada  == null)
                Xunit.Assert.True(false, "Tarefa não encontrado.");

            tarefaAtualizada?.Id.Should().Be(tarefa.Id);
            tarefaAtualizada?.Nome.Should().Be(tarefa.Nome);
            tarefaAtualizada?.Descricao.Should().Be(tarefa.Descricao);
            tarefaAtualizada?.DataHora.Should().Be(tarefa.DataHora);
            tarefaAtualizada?.Prioridade.Should().Be(tarefa.Prioridade);
        }

        [Fact(DisplayName = "Excluir tarefa com sucesso no repositório.")]
        public async Task ShouldRemoveTarefa()
        {
            var tarefa = _fakerTarefa.Generate();

            await _tarefaRepository.Add(tarefa);
            await _tarefaRepository.Delete(tarefa);

            var tarefaRemovida = await _tarefaRepository.GetById((Guid)tarefa.Id);

            if (tarefaRemovida != null)
                Xunit.Assert.True(false, "Tarefa não excluída.");
        }

        [Fact(DisplayName = "Obter todas tarefas.")]
        public async Task ShouldReturnTarefas()
        {
            var tarefas = _fakerTarefa.Generate(5);
            foreach (var tarefa in tarefas)
            {
                await _tarefaRepository.Add(tarefa);
            }

            var tarefasEncontradas = await _tarefaRepository.GetAll();

            if (tarefasEncontradas == null)
                Xunit.Assert.True(false, "Tarefa não encontrada.");

            tarefasEncontradas.Count.Should().BeGreaterOrEqualTo(4);
        }

        [Fact(DisplayName = "Obter tarefa única com base em um Id.")]
        public async Task ShouldReturnSingleTarefa()
        {
            var tarefas = _fakerTarefa.Generate(3);
            foreach (var tarefa in tarefas)
            {
                await _tarefaRepository.Add(tarefa);
            }

            var tarefaId = tarefas[0].Id;
            var tarefaEncontrada = await _tarefaRepository.GetById((Guid) tarefaId);

            if (tarefaEncontrada == null)
                Xunit.Assert.True(false, "Tarefa não encontrada.");

            tarefaEncontrada?.Id.Should().Be(tarefaId);
        }


    }
}