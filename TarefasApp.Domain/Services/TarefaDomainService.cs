using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Interfaces.Repositories;
using TarefasApp.Domain.Interfaces.Services;

namespace TarefasApp.Domain.Services
{

    public class TarefaDomainService : BaseDomainService<Tarefa, Guid>, ITarefaDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TarefaDomainService(IUnitOfWork unitOfWork) : base(unitOfWork.TarefaRepository)
        {
            _unitOfWork = unitOfWork;
        }

        public async override Task Add(Tarefa entity)
        {
            _unitOfWork.TarefaRepository?.Add(entity);
            await _unitOfWork.SaveChanges();
        }

        public async override Task Update(Tarefa entity)
        {
            _unitOfWork.TarefaRepository?.Update(entity);
            await _unitOfWork.SaveChanges();
        }

        public async override Task Delete(Tarefa entity)
        {
            _unitOfWork.TarefaRepository?.Delete(entity);
            await _unitOfWork.SaveChanges();
        }
    }
}
