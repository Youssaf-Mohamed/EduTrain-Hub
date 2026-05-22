using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Windows_Programing.Models
{
    public class Department
    {
        public Department()
        {
            Instructors = new HashSet<Instructor>();
            Courses = new HashSet<Course>();
            Trainees = new HashSet<Trainee>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department Name must be between 2 and 100 characters.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department Manager is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Manager Name must be between 2 and 100 characters.")]
        [Display(Name = "Manager Name")]
        public string Manager { get; set; } = string.Empty;

        // Navigation Properties
        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Trainee> Trainees { get; set; }
    }
}
