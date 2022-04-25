using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public EditModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }

        public SelectList InstructorNames { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department = await _context
                .Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (Department == null)
            {
                return NotFound();
            }

            InstructorNames = new SelectList(_context.Instructors, "ID", "FirstMidName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var departmentToUpdate = await _context
                .Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                return HandleDeletedDepartment();
            }

            _context
                .Entry(departmentToUpdate)
                .Property(d => d.ConcurrencyToken)
                .OriginalValue = Department.ConcurrencyToken;

            if (await TryUpdateModelAsync(
                departmentToUpdate,
                "Department",
                s => s.Name,
                s => s.StartDate,
                s => s.Budget,
                s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. The department was deleted by another user.");

                        return Page();
                    }

                    var dbValues = (Department)databaseEntry.ToObject();
                    await SetDbErrorMessage(dbValues, clientValues);

                    // Save the current ConcurrencyToken so next postback
                    // matches unless an new concurrency issue happens.
                    Department.ConcurrencyToken = dbValues.ConcurrencyToken;
                    // Clear the model error for the next postback.
                    ModelState.Remove($"{nameof(Department)}.{nameof(Department.ConcurrencyToken)}");
                }
            }

            InstructorNames = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);

            return Page();
        }

        private async Task SetDbErrorMessage(Department dbValues, Department clientValues)
        {

            if (dbValues.Name != clientValues.Name)
            {
                ModelState.AddModelError("Department.Name", $"Current value: {dbValues.Name}");
            }
            if (dbValues.Budget != clientValues.Budget)
            {
                ModelState.AddModelError("Department.Budget", $"Current value: {dbValues.Budget:c}");
            }
            if (dbValues.StartDate != clientValues.StartDate)
            {
                ModelState.AddModelError("Department.StartDate", $"Current value: {dbValues.StartDate:d}");
            }
            if (dbValues.InstructorID != clientValues.InstructorID)
            {
                var dbInstructor = await _context.Instructors.FindAsync(dbValues.InstructorID);
                ModelState.AddModelError("Department.InstructorID", $"Current value: {dbInstructor?.FullName}");
            }

            ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you.");
        }

        private IActionResult HandleDeletedDepartment()
        {
            ModelState.AddModelError(string.Empty, "Unable to save. The department was deleted by another user.");

            InstructorNames = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);

            return Page();
        }
    }
}