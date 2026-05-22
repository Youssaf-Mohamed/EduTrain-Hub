using Microsoft.EntityFrameworkCore;
using Windows_Programing.Data;

namespace Windows_Programing.Services
{
    public class CurrentUserService
    {
        private readonly TrainingContext _context;

        public CurrentUserService(TrainingContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetPermissionsAsync(int userId)
        {
            var user = await _context.AppUsers
                .Include(u => u.Role)
                    .ThenInclude(r => r!.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            return user?.Role?.RolePermissions
                .Where(rp => rp.Permission != null)
                .Select(rp => rp.Permission!.Key)
                .Distinct()
                .ToList() ?? new List<string>();
        }
    }
}
