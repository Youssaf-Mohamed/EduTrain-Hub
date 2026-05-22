using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Models;
using Windows_Programing.Security;
using Windows_Programing.ViewModels;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.DashboardView)]
    public class HomeController : Controller
    {
        private readonly TrainingContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(TrainingContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                DepartmentsCount = await _context.Departments.CountAsync(),
                CoursesCount = await _context.Courses.CountAsync(),
                InstructorsCount = await _context.Instructors.CountAsync(),
                TraineesCount = await _context.Trainees.CountAsync(),
                CourseResultsCount = await _context.CourseResults.CountAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
