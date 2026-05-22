using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Windows_Programing.Data;
using Windows_Programing.Models;
using Windows_Programing.Security;
using Windows_Programing.Services;
using Windows_Programing.ViewModels;

namespace Windows_Programing.Controllers
{
    [Authorize(Policy = Permissions.UsersManage)]
    public class SecurityController : Controller
    {
        private readonly TrainingContext _context;

        public SecurityController(TrainingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            var model = new UserListViewModel
            {
                Users = await _context.AppUsers
                    .Include(u => u.Role)
                    .OrderBy(u => u.FullName)
                    .ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> CreateUser()
        {
            return View("UserForm", new UserFormViewModel
            {
                Roles = await GetRolesAsync(),
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Password is required for new users.");
            }

            if (await _context.AppUsers.AnyAsync(u => u.Email.ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already used.");
            }

            if (!ModelState.IsValid)
            {
                model.Roles = await GetRolesAsync();
                return View("UserForm", model);
            }

            _context.AppUsers.Add(new AppUser
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim().ToLower(),
                PasswordHash = PasswordSecurity.HashPassword(model.Password!),
                RoleId = model.RoleId,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User created successfully.";
            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View("UserForm", new UserFormViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                Roles = await GetRolesAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserFormViewModel model)
        {
            if (model.Id == null)
            {
                return NotFound();
            }

            if (await _context.AppUsers.AnyAsync(u => u.Id != model.Id && u.Email.ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already used.");
            }

            if (!ModelState.IsValid)
            {
                model.Roles = await GetRolesAsync();
                return View("UserForm", model);
            }

            var user = await _context.AppUsers.FindAsync(model.Id.Value);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            if (user.Id == currentUserId && !model.IsActive)
            {
                ModelState.AddModelError(nameof(model.IsActive), "You cannot disable your own active session account.");
                model.Roles = await GetRolesAsync();
                return View("UserForm", model);
            }

            user.FullName = model.FullName.Trim();
            user.Email = model.Email.Trim().ToLower();
            user.RoleId = model.RoleId;
            user.IsActive = model.IsActive;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.PasswordHash = PasswordSecurity.HashPassword(model.Password);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Policy = Permissions.RolesManage)]
        public async Task<IActionResult> Roles(int? id)
        {
            var roles = await _context.AppRoles.OrderBy(r => r.Name).ToListAsync();
            var selectedRole = id.HasValue
                ? roles.FirstOrDefault(r => r.Id == id.Value)
                : roles.FirstOrDefault();

            if (selectedRole == null)
            {
                return NotFound();
            }

            var selectedPermissionIds = await _context.AppRolePermissions
                .Where(rp => rp.RoleId == selectedRole.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var permissions = await _context.AppPermissions.OrderBy(p => p.Group).ThenBy(p => p.DisplayName).ToListAsync();
            var model = new RolePermissionsViewModel
            {
                RoleId = selectedRole.Id,
                RoleName = selectedRole.Name,
                Description = selectedRole.Description,
                Roles = roles,
                Permissions = permissions.Select(p => new PermissionCheckItem
                {
                    PermissionId = p.Id,
                    Key = p.Key,
                    DisplayName = p.DisplayName,
                    Group = p.Group,
                    IsSelected = selectedPermissionIds.Contains(p.Id)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.RolesManage)]
        public async Task<IActionResult> Roles(RolePermissionsViewModel model, int[] selectedPermissionIds)
        {
            var role = await _context.AppRoles.FindAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }

            var currentPermissions = await _context.AppRolePermissions
                .Where(rp => rp.RoleId == model.RoleId)
                .ToListAsync();

            if (role.Name == "Super Admin")
            {
                selectedPermissionIds = await _context.AppPermissions.Select(p => p.Id).ToArrayAsync();
            }

            _context.AppRolePermissions.RemoveRange(currentPermissions);
            foreach (var permissionId in selectedPermissionIds.Distinct())
            {
                _context.AppRolePermissions.Add(new AppRolePermission
                {
                    RoleId = model.RoleId,
                    PermissionId = permissionId
                });
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Role permissions updated successfully. Users may need to sign in again to refresh claims.";
            return RedirectToAction(nameof(Roles), new { id = model.RoleId });
        }

        private async Task<List<AppRole>> GetRolesAsync()
        {
            return await _context.AppRoles.OrderBy(r => r.Name).ToListAsync();
        }
    }
}
