using System.Collections.Generic;

namespace Windows_Programing.ViewModels
{
    public class AnalyticsViewModel
    {
        public int TraineesCount { get; set; }
        public int InstructorsCount { get; set; }
        public int DepartmentsCount { get; set; }
        public int CoursesCount { get; set; }
        public int CourseResultsCount { get; set; }

        public double GlobalPassRate { get; set; }

        public List<TraineePerformanceDto> TopTrainees { get; set; } = new();
        public List<CoursePerformanceDto> CoursePerformances { get; set; } = new();
        public List<DepartmentResourceDto> DepartmentResources { get; set; } = new();
        public List<CourseEnrolmentDto> TopCourses { get; set; } = new();
    }

    public class TraineePerformanceDto
    {
        public string TraineeName { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }

    public class CoursePerformanceDto
    {
        public string CourseName { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public int TotalDegree { get; set; }
        public int MinDegree { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public double PassPercentage { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }

    public class DepartmentResourceDto
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public int TraineeCount { get; set; }
        public int CourseCount { get; set; }
        public int InstructorCount { get; set; }
        public double AverageSalary { get; set; }
    }

    public class CourseEnrolmentDto
    {
        public string CourseName { get; set; } = string.Empty;
        public int EnrolledCount { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int Hours { get; set; }
    }
}
