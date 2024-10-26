using Bogus;
using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Exceptions;
using TarefasApp.Domain.Interfaces.Repositories;
using TarefasApp.Domain.Services;
using TarefasApp.Infra.Data.Contexts;
using TarefasApp.Infra.Data.Repositories;
using Xunit;
namespace TarefasApp.IntegrationTests
{
    public class TarefaDomainServiceTest
    {
        private readonly Mock<IValidator<Tarefa>> _validatorMock;
        private readonly TarefaDomainService _tarefaDomainService;

        public TarefaDomainServiceTest( )
        {
            _validatorMock = new Mock<IValidator<Tarefa>>();
            
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TarefasAppTestsDB")
            .Options;

            _tarefaDomainService = new TarefaDomainService((IUnitOfWork) new UnitOfWork(new Infra.Data.Contexts.DataContext(options)), _validatorMock.Object); 
        }

        [Fact(DisplayName = "Adicionar Tarefa com sucesso")]
        public async Task AddAsync_ShouldAddTarefa_WhenValid()
        {
            var tarefa = new Faker<Tarefa>("pt_BR")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                .RuleFor(c => c.DataHora, f => f.Date.Future())
                .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4))
                .Generate();

            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var result = await _tarefaDomainService.Add(tarefa);


            result?.Id.Should().Be(tarefa.Id);
            result?.Nome.Should().Be(tarefa.Nome);
            result?.Descricao.Should().Be(tarefa.Descricao);
            result?.DataHora.Should().Be(tarefa.DataHora);
            result?.Prioridade.Should().Be(tarefa.Prioridade);

        }

        [Fact(DisplayName = "Adicionar tarefa deve falhar na validação")]
        public async Task AddAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var tarefa = new Tarefa { Nome = "Tarefa Teste" };
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Descrição", "A descrição é obrigatória.")
            };

            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            Func<Task> act = async () => await _tarefaDomainService.Add(tarefa);

            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();

            exception.Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("A descrição é obrigatória.");
        }

        [Fact(DisplayName = "Atualizar tarefa com sucesso")]
        public async Task UpdateAsync_ShouldUpdateTarefa_WhenValid()
        {
            var tarefa = new Faker<Tarefa>("pt_BR")
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Nome, f => f.Name.FullName())
                    .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                    .RuleFor(c => c.DataHora, f => f.Date.Future())
                    .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4))
                    .Generate();

            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var resultInclusao = await _tarefaDomainService.Add(tarefa);

            resultInclusao.Nome = "Tarefa Alterada";
            resultInclusao.Descricao = "Descrição Alterada";

            var result = await _tarefaDomainService.Update(tarefa);

            result?.Id.Should().Be(tarefa.Id);
            result?.Nome.Should().Be("Tarefa Alterada");
            result?.Descricao.Should().Be("Descrição Alterada");
            result?.DataHora.Should().Be(tarefa.DataHora);
            result?.Prioridade.Should().Be(tarefa.Prioridade);

        }

        [Fact(DisplayName = "Atualizar tarefa deve falhar na validação")]
        public async Task UpdateAsync_ShouldThrowValidationException_WhenInvalid()
        {
            var tarefa = new Tarefa { Nome = "João" };
            var validationErrors = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Descrição", "A descrição é obrigatória.")
            };

            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult(validationErrors));

            Func<Task> act = async () => await _tarefaDomainService.Update(tarefa);

            var exception = await act.Should().ThrowAsync<FluentValidation.ValidationException>();

            exception.Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("A descrição é obrigatória.");
        }


        [Fact(DisplayName = "Excluir tarefa deve falhar quando tarefa não encontrada")]
        public async Task DeleteAsync_ShouldThrowTarefaNotFoundException_WhenTarefaNotFound()
        {
            var tarefa = new Faker<Tarefa>("pt_BR")
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Nome, f => f.Name.FullName())
                    .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                    .RuleFor(c => c.DataHora, f => f.Date.Future())
                    .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4))
                    .Generate();

                Func <Task> act = async () => await _tarefaDomainService.Delete(tarefa);

            var exception = await act.Should().ThrowAsync<TarefaNotFoundException>()
                .WithMessage($"Tarefa com ID {tarefa.Id} não encontrada.");
        }

        [Fact(DisplayName = "Excluir tarefa com sucesso")]
        public async Task DeleteAsync_ShouldDeleteTarefa_WhenTarefaExists()
        {
            var tarefa = new Faker<Tarefa>("pt_BR")
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Nome, f => f.Name.FullName())
                    .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                    .RuleFor(c => c.DataHora, f => f.Date.Future())
                    .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4))
                    .Generate();

            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var resultinclusao = await _tarefaDomainService.Add(tarefa);

            var result = await _tarefaDomainService.Delete(resultinclusao);

            result?.Id.Should().Be(tarefa.Id);
            result?.Nome.Should().Be(tarefa.Nome);
            result?.Descricao.Should().Be(tarefa.Descricao);
            result?.DataHora.Should().Be(tarefa.DataHora);
            result?.Prioridade.Should().Be(tarefa.Prioridade);
        }

        [Fact(DisplayName = "Consultar Tarefa por ID deve retornar tarefa existente")]
        public async Task GetByIdAsync_ShouldReturnTarefa_WhenTarefaExists()
        {
            var tarefa = new Faker<Tarefa>("pt_BR")
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Nome, f => f.Name.FullName())
                    .RuleFor(c => c.Descricao, f => f.Lorem.Text())
                    .RuleFor(c => c.DataHora, f => f.Date.Future())
                    .RuleFor(c => c.Prioridade, f => new Random().Next(1, 4))
                    .Generate();


            _validatorMock.Setup(v => v.ValidateAsync(tarefa, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var resultinclusao = await _tarefaDomainService.Add(tarefa);

            var result = await _tarefaDomainService.GetById((Guid)tarefa.Id);

            result?.Id.Should().Be(tarefa.Id);
            result?.Nome.Should().Be(tarefa.Nome);
            result?.Descricao.Should().Be(tarefa.Descricao);
            result?.DataHora.Should().Be(tarefa.DataHora);
            result?.Prioridade.Should().Be(tarefa.Prioridade);
        }

        [Fact(DisplayName = "Consultar tarefa por ID deve retornar null quando tarefa não encontrada")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTarefaNotFound()
        {
            var tarefa = new Tarefa { Id = Guid.NewGuid() };

            var result = await _tarefaDomainService.GetById((Guid)tarefa.Id);

            result.Should().BeNull();

        }

        [Fact(DisplayName = "Consultar todas as tarefa")]
        public async Task GetManyAsync_ShouldReturnTarefas_WhenTarefaExist()
        {

            var result = await _tarefaDomainService.GetAll();

            result?.Count().Should().BeGreaterThan(0);

        }


    }
}
