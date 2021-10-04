using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Obsolete.Command
{
    /// <summary>
    ///     Interface for Command Only Dispatcher
    /// </summary>
    public interface ICommandDispatcher
    {

        Task DispatchAsync<T>(T command) where T : ICommand;
    }

    /// <summary>
    ///     Dispatcher will dispatch services dependency injected
    ///     with Command Handler for Command section in CQRS 
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Get, Build and run Command Service using Command Handler with a simple one 
        ///     run function.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var service = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
            await service.Retrieve(command);
        }

    }
}
