using System.Collections.Generic;
using Windows_Programing.Models;

namespace Windows_Programing.ViewModels
{
    public class InstructorSearchViewModel
    {
        public string? Search { get; set; }
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
}
