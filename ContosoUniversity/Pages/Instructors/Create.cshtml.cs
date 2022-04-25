using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly ContosoUniversityContext _context;
        private readonly ILogger<InstructorCoursesPageModel> _logger;

        public CreateModel(ContosoUniversityContext context, ILogger<InstructorCoursesPageModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor { Courses = new List<Course>() };
            PopulateAssignedCourseData(_context, instructor);

            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            var newInstructor = new Instructor();

            if (selectedCourses.Length > 0)
            {
                newInstructor.Courses = new List<Course>();

                _context.Courses.Load();
            }

            foreach (var course in selectedCourses)
            {
                var foundCourse = await _context.Courses.FindAsync(int.Parse(course));
                if (foundCourse != null)
                {
                    newInstructor.Courses.Add(foundCourse);
                }
                else
                {
                    _logger.LogWarning("Course {course} not found", course);
                }
            }

            try
            {
                if (await TryUpdateModelAsync(
                    newInstructor,
                    "Instructor",
                    i => i.FirstMidName,
                    i => i.LastName,
                    i => i.HireDate,
                    i => i.OfficeAssignment))
                {
                    _context.Instructors.Add(newInstructor);

                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            PopulateAssignedCourseData(_context, newInstructor);

            return Page();
        }
    }
}