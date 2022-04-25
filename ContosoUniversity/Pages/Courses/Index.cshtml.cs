using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public IndexModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get;set; }

        public async Task OnGetAsync()
        {
            Courses = await _context
                .Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}