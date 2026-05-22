using System.ComponentModel.DataAnnotations;

namespace Windows_Programing.Models
{
    public class AppRole
    {
        public AppRole()
        {
            Users = new HashSet<AppUser>();
            RolePermissions = new HashSet<AppRolePermission>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; } = string.Empty;

        [StringLength(160)]
        public string Description { get; set; } = string.Empty;

        public bool IsSystemRole { get; set; }

        public virtual ICollection<AppUser> Users { get; set; }
        public virtual ICollection<AppRolePermission> RolePermissions { get; set; }
    }
}
