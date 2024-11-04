using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarefasApp.Infra.Messages.Middleware
{
    public interface IMessageMiddleware
    {
        Task ExecuteAsync(Func<Task> operation, int retryCount = 5, int delayBetweenRetries = 5000, CancellationToken cancellationToken = default);
    }
}
