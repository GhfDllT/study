using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages
{
    public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNames { get; set; }

        public void PopulateDepartmentsDropDownList(
            ContosoUniversityContext _context,
            object selectedDepartment = null)
        {
            var departmentsQuery = _context.Departments.OrderBy(x => x.Name);

            DepartmentNames = new SelectList(
                departmentsQuery.AsNoTracking(),
                "DepartmentID",
                "Name",
                selectedDepartment);
        }
    }
}