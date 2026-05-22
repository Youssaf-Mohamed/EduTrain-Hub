using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;
using Windows_Programing.Models;
using Windows_Programing.Security;

namespace Windows_Programing.Services
{
    public static class AuthSeeder
    {
        public static async Task SeedAsync(TrainingContext context)
        {
            await SeedPermissionsAsync(context);
            await SeedRolesAsync(context);
            await SeedAdminAsync(context);
        }

        private static async Task SeedPermissionsAsync(TrainingContext context)
        {
            var definitions = new Dictionary<string, (string DisplayName, string Group)>
            {
                [Permissions.DashboardView] = ("View dashboard", "Insights"),
                [Permissions.AnalyticsView] = ("View analytics", "Insights"),
                [Permissions.OperationsView] = ("View operations center", "Insights"),
                [Permissions.DepartmentsManage] = ("Manage departments", "Management"),
                [Permissions.CoursesManage] = ("Manage courses", "Management"),
                [Permissions.InstructorsManage] = ("Manage instructors", "Management"),
                [Permissions.TraineesManage] = ("Manage trainees", "Management"),
                [Permissions.ResultsManage] = ("Manage results", "Management"),
                [Permissions.UsersManage] = ("Manage users", "Security"),
                [Permissions.RolesManage] = ("Manage roles and permissions", "Security")
            };

            var existing = await context.AppPermissions.Select(p => p.Key).ToListAsync();
            foreach (var permission in definitions)
            {
                if (existing.Contains(permission.Key))
                {
                    continue;
                }

                context.AppPermissions.Add(new AppPermission
                {
                    Key = permission.Key,
                    DisplayName = permission.Value.DisplayName,
                    Group = permission.Value.Group
                });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(TrainingContext context)
        {
            var roles = new[]
            {
                new AppRole { Name = "Super Admin", Description = "Full control over all modules and security settings.", IsSystemRole = true },
                new AppRole { Name = "Academic Manager", Description = "Manages academic data, operations, and reports.", IsSystemRole = true },
                new AppRole { Name = "Registrar", Description = "Manages trainees and recorded course results.", IsSystemRole = true },
                new AppRole { Name = "Viewer", Description = "Read-only access to dashboards and reports.", IsSystemRole = true }
            };

            foreach (var role in roles)
            {
                if (!await context.AppRoles.AnyAsync(r => r.Name == role.Name))
                {
                    context.AppRoles.Add(role);
                }
            }

            await context.SaveChangesAsync();

            await AssignRolePermissionsAsync(context, "Super Admin", Permissions.All);
            await AssignRolePermissionsAsync(context, "Academic Manager", new[]
            {
                Permissions.DashboardView, Permissions.AnalyticsView, Permissions.OperationsView,
                Permissions.DepartmentsManage, Permissions.CoursesManage, Permissions.InstructorsManage,
                Permissions.TraineesManage, Permissions.ResultsManage
            });
            await AssignRolePermissionsAsync(context, "Registrar", new[]
            {
                Permissions.DashboardView, Permissions.OperationsView,
                Permissions.TraineesManage, Permissions.ResultsManage
            });
            await AssignRolePermissionsAsync(context, "Viewer", new[]
            {
                Permissions.DashboardView, Permissions.AnalyticsView, Permissions.OperationsView
            });
        }

        private static async Task AssignRolePermissionsAsync(TrainingContext context, string roleName, IEnumerable<string> permissionKeys)
        {
            var role = await context.AppRoles.FirstAsync(r => r.Name == roleName);
            var permissions = await context.AppPermissions.Where(p => permissionKeys.Contains(p.Key)).ToListAsync();
            var existing = await context.AppRolePermissions.Where(rp => rp.RoleId == role.Id).ToListAsync();

            foreach (var permission in permissions)
            {
                if (!existing.Any(rp => rp.PermissionId == permission.Id))
                {
                    context.AppRolePermissions.Add(new AppRolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedAdminAsync(TrainingContext context)
        {
            if (await context.AppUsers.AnyAsync())
            {
                return;
            }

            var adminRole = await context.AppRoles.FirstAsync(r => r.Name == "Super Admin");
            context.AppUsers.Add(new AppUser
            {
                FullName = "System Administrator",
                Email = "admin@trainms.local",
                PasswordHash = PasswordSecurity.HashPassword("Admin@12345"),
                RoleId = adminRole.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
        }
    }
}
