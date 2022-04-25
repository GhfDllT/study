using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.ViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversityContext _context;

        public IndexModel(ContosoUniversityContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }

        public int InstructorID { get; set; }

        public int CourseID { get; set; }

        public async Task OnGetAsync(int? id, int? courseId)
        {
            InstructorData= new InstructorIndexData();
            InstructorData.Instructors = await _context
                .Instructors
                .Include(x => x.OfficeAssignment)
                .Include(x => x.Courses)
                    .ThenInclude(x => x.Department)
                .OrderBy(x => x.LastName)
                .ToListAsync();

            if (id != null)
            {
                InstructorID = id.Value;
                var instructor = InstructorData
                    .Instructors
                    .Single(x => x.ID == InstructorID);
                InstructorData.Courses = instructor.Courses;
            }

            if (courseId != null)
            {
                CourseID = courseId.Value;
                var selectedCourse = InstructorData
                    .Courses
                    .Single(x => x.CourseID == CourseID);

                await _context
                    .Entry(selectedCourse)
                    .Collection(x => x.Enrollments)
                    .LoadAsync();

                foreach (var enrollment in selectedCourse.Enrollments)
                {
                    await _context
                        .Entry(enrollment)
                        .Reference(x => x.Student)
                        .LoadAsync();
                }

                InstructorData.Enrollments = selectedCourse.Enrollments;
            }
        }
    }
}