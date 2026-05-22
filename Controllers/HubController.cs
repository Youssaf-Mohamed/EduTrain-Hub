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
    [Authorize(Policy = Permissions.OperationsView)]
    public class HubController : Controller
    {
        private readonly TrainingContext _context;

        public HubController(TrainingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var trainees = await _context.Trainees
                .Include(t => t.Department)
                .OrderBy(t => t.Name)
                .ToListAsync();

            var instructors = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .OrderBy(i => i.Name)
                .ToListAsync();

            var model = new HubViewModel
            {
                Courses = courses,
                Trainees = trainees,
                Instructors = instructors
            };

            return View(model);
        }
    }
}
