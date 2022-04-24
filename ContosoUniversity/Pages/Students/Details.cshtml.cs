#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public DetailsModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context
                .Students
                .Include(x => x.Enrollments)
                .ThenInclude(x => x.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ID == id);

            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}