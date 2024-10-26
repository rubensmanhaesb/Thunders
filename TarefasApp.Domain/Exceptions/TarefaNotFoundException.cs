using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Domain.Exceptions
{
    public class TarefaNotFoundException : Exception
    {
        public TarefaNotFoundException(Guid Id)
            : base($"Tarefa com ID {Id} não encontrada.")
        {

        }
    }
}
