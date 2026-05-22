namespace Windows_Programing.ViewModels
{
    public class OperationsViewModel
    {
        public int PassingResultsCount { get; set; }
        public int FailingResultsCount { get; set; }
        public int CoursesWithoutInstructorsCount { get; set; }
        public int TraineesWithoutResultsCount { get; set; }
        public double AverageScore { get; set; }

        public List<AtRiskTraineeDto> AtRiskTrainees { get; set; } = new();
        public List<RecentResultDto> RecentResults { get; set; } = new();
        public List<CourseCoverageDto> CourseCoverage { get; set; } = new();
    }

    public class AtRiskTraineeDto
    {
        public int TraineeId { get; set; }
        public string TraineeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int FailedCoursesCount { get; set; }
        public double AverageScore { get; set; }
    }

    public class RecentResultDto
    {
        public int ResultId { get; set; }
        public string TraineeName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public int Degree { get; set; }
        public int CourseDegree { get; set; }
        public bool IsPassed { get; set; }
    }

    public class CourseCoverageDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int InstructorCount { get; set; }
        public int ResultCount { get; set; }
        public int Hours { get; set; }
    }
}
