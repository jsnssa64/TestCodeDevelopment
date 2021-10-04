using ASPCoreDevProj.Data;
using DapperProject.Context;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Obsolete.Command
{
    public interface ICommand { }
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Retrieve(TCommand command);
    }

    //  <summary>Abstract class to inherit and build query handler</summary>
    //  <typeparam name="TQuery"> Query must be of IQuery </typeparam>
    //	<typeparam name="TResult"> TResult can be of any time for example IEnumerable<Book> or just Book </typeparam>
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        protected ILogger _logger { get; }
        protected IApplicationDbContext _context { get; }
        public CommandHandler(IApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        //  <summary> User will use to execute Handle(...) and other safety measures</summary>
        //	<param name="Query"> Query To be passed into the handler </param>
        //	<returns> Result of TResult Type </returns>
        public async Task Retrieve(TCommand command) {

            try
            {
                await HandleAsync(command);
            }
            catch(Exception ex)
            {
                throw new Exception("" + ex);
            }
        }

        //  <summary> Handler is to be inherited by the user to get Query results</summary>
        protected abstract Task HandleAsync(TCommand command);
    }

}
