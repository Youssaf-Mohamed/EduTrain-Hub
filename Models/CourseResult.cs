using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Windows_Programing.Models
{
    public class CourseResult
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obtained Degree is required.")]
        [Range(0, 1000, ErrorMessage = "Obtained Degree must be a positive number.")]
        [Display(Name = "Obtained Degree")]
        public int Degree { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        [Display(Name = "Course Name")]
        public int Crs_Id { get; set; }

        [Required(ErrorMessage = "Trainee is required.")]
        [Display(Name = "Trainee Name")]
        public int Trainee_Id { get; set; }

        // Navigation Properties
        [ForeignKey("Crs_Id")]
        public virtual Course? Course { get; set; }

        [ForeignKey("Trainee_Id")]
        public virtual Trainee? Trainee { get; set; }
    }
}
