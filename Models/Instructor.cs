using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windows_Programing.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Instructor Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Instructor Name must be between 3 and 100 characters.")]
        [Display(Name = "Instructor Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Profile Image")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, 1000000, ErrorMessage = "Salary must be a non-negative number.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monthly Salary")]
        public decimal Salary { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int Dept_Id { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        [Display(Name = "Course to Teach")]
        public int Crs_Id { get; set; }

        // Navigation Properties
        [ForeignKey("Dept_Id")]
        public virtual Department? Department { get; set; }

        [ForeignKey("Crs_Id")]
        public virtual Course? Course { get; set; }
    }
}
