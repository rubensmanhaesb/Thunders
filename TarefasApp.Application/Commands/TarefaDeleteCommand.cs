using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Dtos;

namespace TarefasApp.Application.Commands
{

    public class TarefaDeleteCommand : IRequest<TarefaDto>
    {
        public Guid? Id { get; set; }
    }
}
