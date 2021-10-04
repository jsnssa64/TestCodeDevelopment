using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DapperProject.Context;
using CQS.Data.BookQuery;
using MediatR;
using System.Threading;
using CQS.Model.Book;
using ASPCoreDevProj.Model.BookDTO.Book;

namespace ASPCoreDevProj.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<BasicBook> ListOfBooks { get; set; }

        public async Task OnGetAsync()
        {
            GetAllBookQuery obj = new GetAllBookQuery()
            {
                FromPublishDate = 1900,
                TitleMustContain = "1"
            };
            ListOfBooks LOB = await _mediator.Send(obj, new CancellationToken());
            ListOfBooks = LOB.Books.ToList();
        }
    }
}
