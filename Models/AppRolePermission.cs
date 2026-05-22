namespace Windows_Programing.Models
{
    public class AppRolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public virtual AppRole? Role { get; set; }
        public virtual AppPermission? Permission { get; set; }
    }
}
