using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Models;
using Windows_Programing.Security;
using Windows_Programing.Services;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.TraineesManage)]
    public class TraineeController : Controller
    {
        private readonly TrainingContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TraineeController(TrainingContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Trainee
        public async Task<IActionResult> Index(string? search)
        {
            var traineesQuery = _context.Trainees
                .Include(t => t.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                traineesQuery = traineesQuery.Where(t =>
                    t.Name.ToLower().Contains(term) ||
                    t.Grade.ToLower().Contains(term) ||
                    (t.Address != null && t.Address.ToLower().Contains(term)) ||
                    (t.Department != null && t.Department.Name.ToLower().Contains(term)));
            }

            ViewData["Search"] = search;
            var trainees = await traineesQuery.OrderBy(t => t.Name).ToListAsync();
            return View(trainees);
        }

        // GET: Trainee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .Include(t => t.CourseResults)
                    .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // GET: Trainee/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Trainee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Grade,Dept_Id")] Trainee trainee, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    trainee.Image = await ImageStorage.SaveProfileImageAsync(imageFile, _hostEnvironment);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    await PopulateSelectListsAsync(trainee.Dept_Id);
                    return View(trainee);
                }

                _context.Add(trainee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trainee registered successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateSelectListsAsync(trainee.Dept_Id);
            return View(trainee);
        }

        // GET: Trainee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", trainee.Dept_Id);
            return View(trainee);
        }

        // POST: Trainee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Grade,Dept_Id,Image")] Trainee trainee, IFormFile? imageFile)
        {
            if (id != trainee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var oldImage = trainee.Image;
                        trainee.Image = await ImageStorage.SaveProfileImageAsync(imageFile, _hostEnvironment);
                        ImageStorage.DeleteUploadedImage(oldImage, _hostEnvironment);
                    }

                    _context.Update(trainee);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Trainee updated successfully.";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    await PopulateSelectListsAsync(trainee.Dept_Id);
                    return View(trainee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeExists(trainee.Id))
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
            await PopulateSelectListsAsync(trainee.Dept_Id);
            return View(trainee);
        }

        // GET: Trainee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Trainee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee != null)
            {
                ImageStorage.DeleteUploadedImage(trainee.Image, _hostEnvironment);
                _context.Trainees.Remove(trainee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trainee deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(int id)
        {
            return _context.Trainees.Any(e => e.Id == id);
        }

        private async Task PopulateSelectListsAsync(int? selectedDepartment = null)
        {
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", selectedDepartment);
        }
    }
}
