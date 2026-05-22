using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windows_Programing.Models
{
    public class Course
    {
        public Course()
        {
            CourseResults = new HashSet<CourseResult>();
            Instructors = new HashSet<Instructor>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Course Name must be between 2 and 100 characters.")]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total Course Degree is required.")]
        [Range(1, 1000, ErrorMessage = "Degree must be between 1 and 1000.")]
        [Display(Name = "Total Degree")]
        public int Degree { get; set; }

        [Required(ErrorMessage = "Minimum Passing Degree is required.")]
        [Range(0, 1000, ErrorMessage = "Minimum Degree must be between 0 and 1000.")]
        [Display(Name = "Minimum Passing Degree")]
        public int MinDegree { get; set; }

        [Required(ErrorMessage = "Course Hours is required.")]
        [Range(1, 200, ErrorMessage = "Hours must be between 1 and 200.")]
        [Display(Name = "Course Hours")]
        public int Hours { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int Dept_Id { get; set; }

        // Navigation Properties
        [ForeignKey("Dept_Id")]
        public virtual Department? Department { get; set; }

        public virtual ICollection<CourseResult> CourseResults { get; set; }
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
