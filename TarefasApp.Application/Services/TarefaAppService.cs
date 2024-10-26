using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Commands;
using TarefasApp.Application.Dtos;
using TarefasApp.Application.Interfaces;
using TarefasApp.Infra.Storage.Persistence;

namespace TarefasApp.Application.Services
{

    public class TarefaAppService : ITarefaAppService
    {

        private readonly TarefaPersistence _tarefaPersistence;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TarefaAppService(TarefaPersistence tarefaPersistence, IMediator mediator, IMapper mapper)
        {
            _tarefaPersistence = tarefaPersistence;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<TarefaDto> Create(TarefaCreateCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<TarefaDto> Update(TarefaUpdateCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<TarefaDto> Delete(TarefaDeleteCommand command)
        {
            return await _mediator.Send(command);
        }

        public List<TarefaDto>? GetAll()
        {
            var result = _tarefaPersistence.FindAll().Result;
            return _mapper.Map<List<TarefaDto>>(result);
        }

        public TarefaDto? GetById(Guid id)
        {
            var result = _tarefaPersistence.Find(id).Result;
            return _mapper.Map<TarefaDto>(result);
        }
    }
}
