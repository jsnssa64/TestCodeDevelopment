using ASPCoreDevProj.Data.Pipeline;
using ASPCoreDevProj.Model.BookDTO.Book;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DapperProject.Context;
using Domain.Data.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQS.Data.BookQuery
{

    //  Filter for query Builder
    public class GetDetailedBookQuery : IRequest<BookWithGenreAndAuthor>, ILoggerPipeline
    {
        public int Id { get; set; }
    }

    public class GetBookQueryHandler : IRequestHandler<GetDetailedBookQuery, BookWithGenreAndAuthor> 

    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetBookQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookWithGenreAndAuthor> Handle(GetDetailedBookQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Book> getBook = _context.Books
                                                    .Include(b => b.BooksGenres)
                                                    .Include(b => b.BooksAuthors)
                                                    .AsNoTracking()
                                                    .AsSplitQuery()
                                                        .Where(b => b.Id == request.Id);
            BookWithGenreAndAuthor book = await getBook.ProjectTo<BookWithGenreAndAuthor>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
            
            return book;
        }
    }
}
