using System.ComponentModel.DataAnnotations;
using Windows_Programing.Models;

namespace Windows_Programing.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class UserListViewModel
    {
        public List<AppUser> Users { get; set; } = new();
    }

    public class UserFormViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [StringLength(80, MinimumLength = 8)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;
        public List<AppRole> Roles { get; set; } = new();
    }

    public class RolePermissionsViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PermissionCheckItem> Permissions { get; set; } = new();
        public List<AppRole> Roles { get; set; } = new();
    }

    public class PermissionCheckItem
    {
        public int PermissionId { get; set; }
        public string Key { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
