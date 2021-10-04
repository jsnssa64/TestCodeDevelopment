using ASPCoreDevProj.Data;
using ASPCoreDevProj.Data.BookQuery;
using CQS.Data.BookQuery;
using CQS.Model;
using DapperProject.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ASPCoreDevProj.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        protected IMediator _mediator { get; }

        /*  Obsolete Dispatcher CQRS
         * public IQueryDispatcher _queryDispatcher { get; set; }
        public ICommandDispatcher _commandDispatcher { get; set; }*/

        public IndexModel(  ILogger<IndexModel> logger, 
                            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public void OnGet()
        {
            /*  Custom Dispatcher Replaced By MediatR */
            /*FullBookStore result  = await _queryDispatcher.DispatchAsync<GetAllBookQuery, FullBookStore>(query);
            DeleteBookCommand commandQuery = new DeleteBookCommand(new Guid());
            await _commandDispatcher.DispatchAsync<DeleteBookCommand>(commandQuery);*/
        }
    }
}
