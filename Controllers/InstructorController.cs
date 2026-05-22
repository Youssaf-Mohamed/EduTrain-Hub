using System;
using System.IO;
using System.Linq;
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
using Windows_Programing.ViewModels;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.InstructorsManage)]
    public class InstructorController : Controller
    {
        private readonly TrainingContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public InstructorController(TrainingContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Instructor
        // Example: /Instructor/Index?search=nour
        public async Task<IActionResult> Index(string? search)
        {
            var instructorsQuery = _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                instructorsQuery = instructorsQuery.Where(i =>
                    i.Name.ToLower().Contains(term) ||
                    (i.Address != null && i.Address.ToLower().Contains(term)) ||
                    (i.Department != null && i.Department.Name.ToLower().Contains(term)) ||
                    (i.Course != null && i.Course.Name.ToLower().Contains(term)));
            }

            var model = new InstructorSearchViewModel
            {
                Search = search,
                Instructors = await instructorsQuery.OrderBy(i => i.Name).ToListAsync()
            };

            return View(model);
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Salary,Address,Dept_Id,Crs_Id")] Instructor instructor, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    instructor.Image = await ImageStorage.SaveProfileImageAsync(imageFile, _hostEnvironment);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    await PopulateSelectListsAsync(instructor.Dept_Id, instructor.Crs_Id);
                    return View(instructor);
                }

                _context.Add(instructor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Instructor created successfully.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateSelectListsAsync(instructor.Dept_Id, instructor.Crs_Id);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name", instructor.Dept_Id);
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", instructor.Crs_Id);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Salary,Address,Dept_Id,Crs_Id,Image")] Instructor instructor, IFormFile? imageFile)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var oldImage = instructor.Image;
                        instructor.Image = await ImageStorage.SaveProfileImageAsync(imageFile, _hostEnvironment);
                        ImageStorage.DeleteUploadedImage(oldImage, _hostEnvironment);
                    }

                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Instructor updated successfully.";
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Image", ex.Message);
                    await PopulateSelectListsAsync(instructor.Dept_Id, instructor.Crs_Id);
                    return View(instructor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            await PopulateSelectListsAsync(instructor.Dept_Id, instructor.Crs_Id);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                ImageStorage.DeleteUploadedImage(instructor.Image, _hostEnvironment);
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Instructor deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }

        private async Task PopulateSelectListsAsync(int? selectedDepartment = null, int? selectedCourse = null)
        {
            ViewData["Dept_Id"] = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", selectedDepartment);
            ViewData["Crs_Id"] = new SelectList(await _context.Courses.OrderBy(c => c.Name).ToListAsync(), "Id", "Name", selectedCourse);
        }
    }
}
