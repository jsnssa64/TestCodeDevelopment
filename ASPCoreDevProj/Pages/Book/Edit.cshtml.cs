using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CQS.Data.BookQuery;
using MediatR;
using System.Threading;
using AutoMapper;
using ASPCoreDevProj.Model.BookDTO.Book;
using ASPCoreDevProj.Model.BookDTO.Book.Interface;
using System.Collections.Generic;
using ASPCoreDevProj.Model.BookDTO.Author;
using ASPCoreDevProj.Model.BookDTO.Genre;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using DapperProject.Context;
using Domain.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ASPCoreDevProj.Pages.Book
{
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        public EditModel(IApplicationDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        [BindProperty]
        public BookWithGenreAndAuthor Book { get; set; }
        public string JGenres { get; set; }
        public string JAuthors { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GetDetailedBookQuery identity = new GetDetailedBookQuery()
            {
                Id = (int)id
            };

            Book = await _mediator.Send(identity, new CancellationToken());

            IEnumerable<Genre> genreList = await _context.Genres.ToListAsync();
            IEnumerable<Author> authorList = await _context.Authors.ToListAsync();

            if (Book == null)
            {
                return NotFound();
            }

            JGenres = GetJson<Genre>(genreList);
            JAuthors = GetJson<Author>(authorList);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            AuthorBasic Author1 = new AuthorBasic() {
                Name = "AuthorAddition1"
            };

            AuthorBasic Author2 = new AuthorBasic() {
                Name = "AuthorAddition2"
            };

            GenreBasic Genre1 = new GenreBasic() {
                Name = "GenreAddition1"
            };

            GenreBasic Genre2 = new GenreBasic() {
                Name = "GenreAddition2"
            };

            Book.Genres.Add(Genre1);
            Book.Genres.Add(Genre2);
            Book.Authors.Add(Author1);
            Book.Authors.Add(Author2);

            UpdateBookCommand updateCommand = new UpdateBookCommand()
            {
                book = _mapper.Map<Domain.Data.Model.Book>(Book)
            };

            Unit result = await _mediator.Send(updateCommand, new CancellationToken());

            return RedirectToPage("./Index");
        }

        public string GetJson<T>(IEnumerable<T> Object) => JsonSerializer.Serialize(Object);

        /*public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }*/
        /*private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }*/
    }
}
