using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarefasApp.Infra.Messages.Middleware;

namespace TarefasApp.Infra.Messages.Middleware
{
    public class MessageMiddleware : IMessageMiddleware
    {
        private readonly ILogger<MessageMiddleware> _logger;

        public MessageMiddleware(ILogger<MessageMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(Func<Task> operation, int retryCount = 5, int delayBetweenRetries = 5000, CancellationToken cancellationToken = default)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount,
                    attempt => TimeSpan.FromMilliseconds(delayBetweenRetries),
                    (exception, timeSpan, attempt, context) =>
                    {
                        _logger.LogError($"Erro ao executar a operação na tentativa {attempt} de {retryCount}: {exception.Message}");
                    });

            await retryPolicy.ExecuteAsync(async () =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Operação cancelada.");
                    return;
                }

                await operation();
            });
        }
    }

}