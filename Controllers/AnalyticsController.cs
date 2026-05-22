using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Security;
using Windows_Programing.ViewModels;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.AnalyticsView)]
    public class AnalyticsController : Controller
    {
        private readonly TrainingContext _context;

        public AnalyticsController(TrainingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalTrainees = await _context.Trainees.CountAsync();
            var totalInstructors = await _context.Instructors.CountAsync();
            var totalDepts = await _context.Departments.CountAsync();
            var totalCourses = await _context.Courses.CountAsync();
            var totalResults = await _context.CourseResults.CountAsync();

            // Calculate global passing rate
            var resultsWithCourse = await _context.CourseResults.Include(cr => cr.Course).ToListAsync();
            var passingCount = resultsWithCourse.Count(cr => cr.Degree >= (cr.Course?.MinDegree ?? 0));
            var globalPassRate = totalResults > 0 ? (double)passingCount / totalResults * 100 : 0;

            // 1. Top 5 Trainees
            var traineesWithGrades = await _context.Trainees
                .Include(t => t.Department)
                .Include(t => t.CourseResults)
                .ToListAsync();

            var topTrainees = traineesWithGrades
                .Where(t => t.CourseResults.Any())
                .Select(t => new TraineePerformanceDto
                {
                    TraineeName = t.Name,
                    AverageScore = t.CourseResults.Average(cr => cr.Degree),
                    DepartmentName = t.Department?.Name ?? "General",
                    Grade = t.Grade,
                    Image = t.Image ?? string.Empty
                })
                .OrderByDescending(t => t.AverageScore)
                .Take(5)
                .ToList();

            // 2. Course Performance breakdown
            var coursesWithResults = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.CourseResults)
                .ToListAsync();

            var coursePerformances = coursesWithResults
                .Select(c => {
                    var avg = c.CourseResults.Any() ? c.CourseResults.Average(cr => cr.Degree) : 0;
                    var total = c.CourseResults.Count;
                    var passed = c.CourseResults.Count(cr => cr.Degree >= c.MinDegree);
                    var failed = total - passed;
                    var percentage = total > 0 ? (double)passed / total * 100 : 0;

                    return new CoursePerformanceDto
                    {
                        CourseName = c.Name,
                        AverageScore = avg,
                        TotalDegree = c.Degree,
                        MinDegree = c.MinDegree,
                        PassCount = passed,
                        FailCount = failed,
                        PassPercentage = percentage,
                        DepartmentName = c.Department?.Name ?? "Unassigned"
                    };
                })
                .OrderByDescending(cp => cp.PassPercentage)
                .ToList();

            // 3. Department resource allocations
            var departmentsInfo = await _context.Departments
                .Include(d => d.Trainees)
                .Include(d => d.Courses)
                .Include(d => d.Instructors)
                .ToListAsync();

            var departmentResources = departmentsInfo
                .Select(d => {
                    var avgSalary = d.Instructors.Any() ? (double)d.Instructors.Average(i => i.Salary) : 0.0;
                    return new DepartmentResourceDto
                    {
                        DepartmentName = d.Name,
                        ManagerName = d.Manager,
                        TraineeCount = d.Trainees.Count,
                        CourseCount = d.Courses.Count,
                        InstructorCount = d.Instructors.Count,
                        AverageSalary = avgSalary
                    };
                })
                .ToList();

            // 4. Highly Popular Courses
            var topCourses = coursesWithResults
                .Select(c => new CourseEnrolmentDto
                {
                    CourseName = c.Name,
                    EnrolledCount = c.CourseResults.Count,
                    DepartmentName = c.Department?.Name ?? "Unassigned",
                    Hours = c.Hours
                })
                .OrderByDescending(ce => ce.EnrolledCount)
                .Take(5)
                .ToList();

            var model = new AnalyticsViewModel
            {
                TraineesCount = totalTrainees,
                InstructorsCount = totalInstructors,
                DepartmentsCount = totalDepts,
                CoursesCount = totalCourses,
                CourseResultsCount = totalResults,
                GlobalPassRate = globalPassRate,
                TopTrainees = topTrainees,
                CoursePerformances = coursePerformances,
                DepartmentResources = departmentResources,
                TopCourses = topCourses
            };

            return View(model);
        }
    }
}
