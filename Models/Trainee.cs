using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windows_Programing.Models
{
    public class Trainee
    {
        public Trainee()
        {
            CourseResults = new HashSet<CourseResult>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Trainee Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Trainee Name must be between 3 and 100 characters.")]
        [Display(Name = "Trainee Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Profile Image")]
        public string? Image { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Trainee Grade is required.")]
        [StringLength(50, ErrorMessage = "Grade cannot exceed 50 characters.")]
        [Display(Name = "Grade/Level")]
        public string Grade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int Dept_Id { get; set; }

        // Navigation Properties
        [ForeignKey("Dept_Id")]
        public virtual Department? Department { get; set; }

        public virtual ICollection<CourseResult> CourseResults { get; set; }
    }
}
