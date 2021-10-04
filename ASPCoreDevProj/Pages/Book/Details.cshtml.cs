using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DapperProject.Context;

namespace ASPCoreDevProj.Pages.Book
{
    public class DetailsModel : PageModel
    {
        private readonly DapperProject.Context.ApplicationDbContext _context;

        public DetailsModel(DapperProject.Context.ApplicationDbContext context)
        {
            _context = context;
        }

        public Domain.Data.Model.Book Book { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);

            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
