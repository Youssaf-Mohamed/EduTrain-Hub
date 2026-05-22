using System.Collections.Generic;
using Windows_Programing.Models;

namespace Windows_Programing.ViewModels
{
    public class HubViewModel
    {
        public List<Course> Courses { get; set; } = new();
        public List<Trainee> Trainees { get; set; } = new();
        public List<Instructor> Instructors { get; set; } = new();
    }
}
