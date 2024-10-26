using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Exceptions;
using TarefasApp.Domain.Extensions;
using TarefasApp.Domain.Interfaces.Repositories;
using TarefasApp.Domain.Interfaces.Services;

namespace TarefasApp.Domain.Services
{

    public class TarefaDomainService : BaseDomainService<Tarefa, Guid>, ITarefaDomainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IValidator<Tarefa> _validator;

        public TarefaDomainService(IUnitOfWork unitOfWork, IValidator<Tarefa> validator) : base(unitOfWork.TarefaRepository)
        {
            _unitOfWork = unitOfWork;
            _tarefaRepository = unitOfWork.TarefaRepository;
            _validator = validator;
        }

        public async override Task<Tarefa> Add(Tarefa entity)
        {
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _unitOfWork.TarefaRepository?.Add(entity);
            await _unitOfWork.SaveChanges();
            return entity;
        }

        public async override Task<Tarefa> Update(Tarefa entity)
        {
            var validationResult = await _validator.ValidateAsync(entity);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _unitOfWork.TarefaRepository?.Update(entity);
            await _unitOfWork.SaveChanges();
            return entity;
        }

        public async override Task<Tarefa> Delete(Tarefa entity)
        {
            var tarefa = await _tarefaRepository.GetById((Guid)entity.Id);
            if (tarefa == null)
                throw new TarefaNotFoundException((Guid)entity.Id);

            await _unitOfWork.TarefaRepository?.Delete(tarefa);
            await _unitOfWork.SaveChanges();
            return tarefa;
        }
    }
}
