using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Interfaces.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Domain.Validations
{
    public class TarefaValidator : AbstractValidator<Tarefa>
    {
        private Guid? _currentTarefaId;

        public TarefaValidator()
        {
            ConfigureRules();
        }

        public void SetCurrentTarefaId(Guid tarefaId)
        {
            _currentTarefaId = tarefaId;
        }

        private void ConfigureRules()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O id é obrigatório")
                .Must(id => id != Guid.Empty).WithMessage("O id não pode ser igual ao valor padrão.");

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(2, 150).WithMessage("O nome deve ter de 2 a 150 caracteres.");

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(c => c.Prioridade)
                .NotEmpty().WithMessage("A prioridade é obrigatória.");

            RuleFor(c => c.DataHora)
                .NotEmpty().WithMessage("A data e hora são obrigatórias.");


        }

    }
}
