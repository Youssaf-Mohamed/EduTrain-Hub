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
    [Authorize(Policy = Permissions.ResultsManage)]
    public class CourseResultController : Controller
    {
        private readonly TrainingContext _context;

        public CourseResultController(TrainingContext context)
        {
            _context = context;
        }

        // GET: CourseResult
        public async Task<IActionResult> Index(string? search, string? status)
        {
            var resultsQuery = _context.CourseResults
                .Include(cr => cr.Course)
                .Include(cr => cr.Trainee)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                resultsQuery = resultsQuery.Where(cr =>
                    (cr.Trainee != null && cr.Trainee.Name.ToLower().Contains(term)) ||
                    (cr.Course != null && cr.Course.Name.ToLower().Contains(term)));
            }

            if (status == "passed")
            {
                resultsQuery = resultsQuery.Where(cr => cr.Course != null && cr.Degree >= cr.Course.MinDegree);
            }
            else if (status == "failed")
            {
                resultsQuery = resultsQuery.Where(cr => cr.Course != null && cr.Degree < cr.Course.MinDegree);
            }

            ViewData["Search"] = search;
            ViewData["Status"] = status;
            var results = await resultsQuery
                .OrderBy(cr => cr.Trainee != null ? cr.Trainee.Name : string.Empty)
                .ThenBy(cr => cr.Course != null ? cr.Course.Name : string.Empty)
                .ToListAsync();
            return View(results);
        }

        // GET: CourseResult/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseResult = await _context.CourseResults
                .Include(cr => cr.Course)
                .Include(cr => cr.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (courseResult == null)
            {
                return NotFound();
            }

            return View(courseResult);
        }

        // GET: CourseResult/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");
            ViewData["Trainee_Id"] = new SelectList(await _context.Trainees.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: CourseResult/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Degree,Crs_Id,Trainee_Id")] CourseResult courseResult)
        {
            await ValidateCourseResultAsync(courseResult);

            if (ModelState.IsValid)
            {
                _context.Add(courseResult);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Grade recorded successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", courseResult.Crs_Id);
            ViewData["Trainee_Id"] = new SelectList(await _context.Trainees.ToListAsync(), "Id", "Name", courseResult.Trainee_Id);
            return View(courseResult);
        }

        // GET: CourseResult/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseResult = await _context.CourseResults.FindAsync(id);
            if (courseResult == null)
            {
                return NotFound();
            }
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", courseResult.Crs_Id);
            ViewData["Trainee_Id"] = new SelectList(await _context.Trainees.ToListAsync(), "Id", "Name", courseResult.Trainee_Id);
            return View(courseResult);
        }

        // POST: CourseResult/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Degree,Crs_Id,Trainee_Id")] CourseResult courseResult)
        {
            if (id != courseResult.Id)
            {
                return NotFound();
            }

            await ValidateCourseResultAsync(courseResult, id);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseResult);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Grade updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseResultExists(courseResult.Id))
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
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", courseResult.Crs_Id);
            ViewData["Trainee_Id"] = new SelectList(await _context.Trainees.ToListAsync(), "Id", "Name", courseResult.Trainee_Id);
            return View(courseResult);
        }

        // GET: CourseResult/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseResult = await _context.CourseResults
                .Include(cr => cr.Course)
                .Include(cr => cr.Trainee)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseResult == null)
            {
                return NotFound();
            }

            return View(courseResult);
        }

        // POST: CourseResult/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseResult = await _context.CourseResults.FindAsync(id);
            if (courseResult != null)
            {
                _context.CourseResults.Remove(courseResult);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Grade deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CourseResultExists(int id)
        {
            return _context.CourseResults.Any(e => e.Id == id);
        }

        private async Task ValidateCourseResultAsync(CourseResult courseResult, int? currentId = null)
        {
            var course = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == courseResult.Crs_Id);
            if (course == null)
            {
                return;
            }

            if (courseResult.Degree > course.Degree)
            {
                ModelState.AddModelError(nameof(CourseResult.Degree), $"Obtained degree cannot exceed the course total degree ({course.Degree}).");
            }

            var duplicateExists = await _context.CourseResults.AnyAsync(cr =>
                cr.Crs_Id == courseResult.Crs_Id &&
                cr.Trainee_Id == courseResult.Trainee_Id &&
                (!currentId.HasValue || cr.Id != currentId.Value));

            if (duplicateExists)
            {
                ModelState.AddModelError(string.Empty, "This trainee already has a recorded grade for the selected course.");
            }
        }
    }
}
