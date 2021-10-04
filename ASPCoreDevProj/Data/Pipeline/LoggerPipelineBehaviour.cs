using CQS.Data.BookQuery;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Data.Pipeline
{
    public interface ILoggerPipeline { }

    public class LoggerPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ILoggerPipeline
    {
        private readonly ILogger<LoggerPipelineBehaviour<TRequest, TResponse>> logger;
        public LoggerPipelineBehaviour(ILogger<LoggerPipelineBehaviour<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //logger.ExecutingRequest(logger, typeof(TRequest).Name);
            Console.WriteLine("Logging ->");
            var response = await next();
            Console.WriteLine("<- Logging");

            //logger.ExecutedRequest(logger, typeof(TRequest).Name);

            return response;
        }
    }
}
