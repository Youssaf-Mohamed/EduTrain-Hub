using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Models;
using Windows_Programing.Security;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.CoursesManage)]
    public class CourseController : Controller
    {
        private readonly TrainingContext _context;

        public CourseController(TrainingContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index(string? search)
        {
            var coursesQuery = _context.Courses
                .Include(c => c.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                coursesQuery = coursesQuery.Where(c =>
                    c.Name.ToLower().Contains(term) ||
                    (c.Department != null && c.Department.Name.ToLower().Contains(term)));
            }

            ViewData["Search"] = search;
            var courses = await coursesQuery.OrderBy(c => c.Name).ToListAsync();
            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.CourseResults)
                    .ThenInclude(cr => cr.Trainee)
                .Include(c => c.Instructors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Degree,MinDegree,Hours,Dept_Id")] Course course)
        {
            ValidateCourseDegrees(course);

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", course.Dept_Id);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", course.Dept_Id);
            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Degree,MinDegree,Hours,Dept_Id")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            ValidateCourseDegrees(course);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", course.Dept_Id);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                try
                {
                    _context.Courses.Remove(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course deleted successfully.";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Course cannot be deleted while linked to instructors or results.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private void ValidateCourseDegrees(Course course)
        {
            if (course.MinDegree > course.Degree)
            {
                ModelState.AddModelError(nameof(Course.MinDegree), "Minimum passing degree cannot be greater than total degree.");
            }
        }
    }
}
