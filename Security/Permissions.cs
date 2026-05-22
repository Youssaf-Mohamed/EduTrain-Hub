namespace Windows_Programing.Security
{
    public static class Permissions
    {
        public const string DashboardView = "dashboard.view";
        public const string AnalyticsView = "analytics.view";
        public const string OperationsView = "operations.view";

        public const string DepartmentsManage = "departments.manage";
        public const string CoursesManage = "courses.manage";
        public const string InstructorsManage = "instructors.manage";
        public const string TraineesManage = "trainees.manage";
        public const string ResultsManage = "results.manage";

        public const string UsersManage = "users.manage";
        public const string RolesManage = "roles.manage";

        public static readonly string[] All =
        {
            DashboardView,
            AnalyticsView,
            OperationsView,
            DepartmentsManage,
            CoursesManage,
            InstructorsManage,
            TraineesManage,
            ResultsManage,
            UsersManage,
            RolesManage
        };
    }
}
