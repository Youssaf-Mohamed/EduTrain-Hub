using System.ComponentModel.DataAnnotations;

namespace Windows_Programing.Models
{
    public class AppPermission
    {
        public AppPermission()
        {
            RolePermissions = new HashSet<AppRolePermission>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(90)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [StringLength(90)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string Group { get; set; } = string.Empty;

        public virtual ICollection<AppRolePermission> RolePermissions { get; set; }
    }
}
