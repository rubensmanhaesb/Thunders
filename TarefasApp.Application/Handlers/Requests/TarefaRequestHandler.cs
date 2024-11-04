using AutoMapper;
using MediatR;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Commands;
using TarefasApp.Application.Dtos;
using TarefasApp.Application.Handlers.Notifications;
using TarefasApp.Domain.Entities;
using TarefasApp.Domain.Interfaces.Services;
using TarefasApp.Infra.Messages.Models;
using TarefasApp.Infra.Messages.Producers;

namespace TarefasApp.Application.Handlers.Requests
{

    public class TarefaRequestHandler :
        IRequestHandler<TarefaCreateCommand, TarefaDto>,
        IRequestHandler<TarefaUpdateCommand, TarefaDto>,
        IRequestHandler<TarefaDeleteCommand, TarefaDto>
    {
        //atributo
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ITarefaDomainService _tarefaDomainService;
        private readonly MessageProducer _messageProducer;


        public TarefaRequestHandler(IMediator mediator, IMapper mapper, ITarefaDomainService tarefaDomainService, MessageProducer messageProducer)
        {
            _mediator = mediator;
            _mapper = mapper;
            _tarefaDomainService = tarefaDomainService;
            _messageProducer = messageProducer;
        }
        private void SendMessage(TarefaDto tarefaDto, string subject)
        {
            _messageProducer.SendMessageAsync(new EmailMessageModel
            {
                Subject = subject,
                Body = Newtonsoft.Json.JsonConvert.SerializeObject(tarefaDto, Formatting.Indented)
            });
        }
        public async Task<TarefaDto> Handle(TarefaCreateCommand request, CancellationToken cancellationToken)
        {

            var tarefa = _mapper.Map<Tarefa>(request);
            await _tarefaDomainService.Add(tarefa);


            var tarefaDto = _mapper.Map<TarefaDto>(tarefa);
            var tarefaNotification = new TarefaNotification
            {
                Tarefa = tarefaDto,
                Action = TarefaNotificationAction.TarefaCriada
            };

            await _mediator.Publish(tarefaNotification);

            SendMessage(tarefaDto, $"Nova tarefa criada com sucesso em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");


            return tarefaDto;
        }

        public async Task<TarefaDto> Handle(TarefaUpdateCommand request, CancellationToken cancellationToken)
        {

            var tarefa = _mapper.Map<Tarefa>(request);
            await _tarefaDomainService.Update(tarefa);


            var tarefaDto = _mapper.Map<TarefaDto>(tarefa);
            var tarefaNotification = new TarefaNotification
            {
                Tarefa = tarefaDto,
                Action = TarefaNotificationAction.TarefaAlterada
            };

            await _mediator.Publish(tarefaNotification);
          
            SendMessage(tarefaDto, $"Tarefa alterada com sucesso em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");

            return tarefaDto;
        }

        public async Task<TarefaDto> Handle(TarefaDeleteCommand request, CancellationToken cancellationToken)
        {

            var tarefa = await _tarefaDomainService.GetById(request.Id.Value);
            await _tarefaDomainService.Delete(new Tarefa() { Id = request.Id.Value });


            var tarefaDto = _mapper.Map<TarefaDto>(tarefa);
            var tarefaNotification = new TarefaNotification
            {
                Tarefa = tarefaDto,
                Action = TarefaNotificationAction.TarefaExcluida
            };

            await _mediator.Publish(tarefaNotification);

            SendMessage(tarefaDto, $"Tarefa excluída com sucesso em {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");

            return tarefaDto;
        }
    }
}
