using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Security;
using Windows_Programing.ViewModels;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.OperationsView)]
    public class OperationsController : Controller
    {
        private readonly TrainingContext _context;

        public OperationsController(TrainingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _context.CourseResults
                .Include(cr => cr.Course)
                .Include(cr => cr.Trainee)
                    .ThenInclude(t => t!.Department)
                .OrderByDescending(cr => cr.Id)
                .ToListAsync();

            var courses = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .Include(c => c.CourseResults)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var trainees = await _context.Trainees
                .Include(t => t.Department)
                .Include(t => t.CourseResults)
                    .ThenInclude(cr => cr.Course)
                .OrderBy(t => t.Name)
                .ToListAsync();

            var passingCount = results.Count(cr => cr.Course != null && cr.Degree >= cr.Course.MinDegree);
            var failingCount = results.Count(cr => cr.Course != null && cr.Degree < cr.Course.MinDegree);

            var model = new OperationsViewModel
            {
                PassingResultsCount = passingCount,
                FailingResultsCount = failingCount,
                CoursesWithoutInstructorsCount = courses.Count(c => !c.Instructors.Any()),
                TraineesWithoutResultsCount = trainees.Count(t => !t.CourseResults.Any()),
                AverageScore = results.Any() ? results.Average(cr => cr.Degree) : 0,
                RecentResults = results.Take(8).Select(cr => new RecentResultDto
                {
                    ResultId = cr.Id,
                    TraineeName = cr.Trainee?.Name ?? "Unassigned",
                    CourseName = cr.Course?.Name ?? "Unassigned",
                    Degree = cr.Degree,
                    CourseDegree = cr.Course?.Degree ?? 100,
                    IsPassed = cr.Course != null && cr.Degree >= cr.Course.MinDegree
                }).ToList(),
                AtRiskTrainees = trainees
                    .Where(t => t.CourseResults.Any(cr => cr.Course != null && cr.Degree < cr.Course.MinDegree))
                    .Select(t => new AtRiskTraineeDto
                    {
                        TraineeId = t.Id,
                        TraineeName = t.Name,
                        DepartmentName = t.Department?.Name ?? "General",
                        FailedCoursesCount = t.CourseResults.Count(cr => cr.Course != null && cr.Degree < cr.Course.MinDegree),
                        AverageScore = t.CourseResults.Any() ? t.CourseResults.Average(cr => cr.Degree) : 0
                    })
                    .OrderByDescending(t => t.FailedCoursesCount)
                    .ThenBy(t => t.AverageScore)
                    .Take(6)
                    .ToList(),
                CourseCoverage = courses.Select(c => new CourseCoverageDto
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    DepartmentName = c.Department?.Name ?? "General",
                    InstructorCount = c.Instructors.Count,
                    ResultCount = c.CourseResults.Count,
                    Hours = c.Hours
                }).ToList()
            };

            return View(model);
        }
    }
}
