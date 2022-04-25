using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoUniversity.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList;

        public void PopulateAssignedCourseData(
            ContosoUniversityContext context,
            Instructor instructor)
        {
            var allCourses = context.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(x => x.CourseID));

            AssignedCourseDataList = allCourses
                .Select(x => new AssignedCourseData
                {
                    CourseID = x.CourseID,
                    Title = x.Title,
                    Assigned = instructorCourses.Contains(x.CourseID)
                })
                .ToList();
        }
    }
}