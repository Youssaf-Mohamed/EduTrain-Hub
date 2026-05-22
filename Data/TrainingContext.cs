using Microsoft.EntityFrameworkCore;
using Windows_Programing.Models;

namespace Windows_Programing.Data
{
    public class TrainingContext : DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<CourseResult> CourseResults { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<AppRolePermission> AppRolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for Instructor Salary
            modelBuilder.Entity<Instructor>()
                .Property(i => i.Salary)
                .HasConversion<double>(); // SQLite doesn't natively support decimal precision well, so double/float conversion or keeping it is fine. EF Sqlite handles decimal as text or real. Converting is safe.

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<AppRole>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<AppPermission>()
                .HasIndex(p => p.Key)
                .IsUnique();

            modelBuilder.Entity<AppRolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<AppRolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppRolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1. Department has many Instructors
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Department)
                .WithMany(d => d.Instructors)
                .HasForeignKey(i => i.Dept_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Department has many Courses
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.Dept_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Department has many Trainees
            modelBuilder.Entity<Trainee>()
                .HasOne(t => t.Department)
                .WithMany(d => d.Trainees)
                .HasForeignKey(t => t.Dept_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Instructor teaches one Course
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Course)
                .WithMany(c => c.Instructors)
                .HasForeignKey(i => i.Crs_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. CourseResult relationships
            modelBuilder.Entity<CourseResult>()
                .HasOne(cr => cr.Course)
                .WithMany(c => c.CourseResults)
                .HasForeignKey(cr => cr.Crs_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseResult>()
                .HasOne(cr => cr.Trainee)
                .WithMany(t => t.CourseResults)
                .HasForeignKey(cr => cr.Trainee_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Seed Data ---

            // Seed Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Software Development (SD)", Manager = "Dr. Ahmed Ali" },
                new Department { Id = 2, Name = "Open Source (OS)", Manager = "Dr. Sarah Kamal" },
                new Department { Id = 3, Name = "Artificial Intelligence (AI)", Manager = "Dr. Nour Mansour" },
                new Department { Id = 4, Name = "Cyber Security (CS)", Manager = "Dr. Kareem Fouad" },
                new Department { Id = 5, Name = "Data Science (DS)", Manager = "Dr. Laila Hassan" },
                new Department { Id = 6, Name = "Cloud Computing & DevOps", Manager = "Dr. Mostafa Kamel" }
            );

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "C# Web Application Development", Degree = 100, MinDegree = 50, Hours = 60, Dept_Id = 1 },
                new Course { Id = 2, Name = "ASP.NET Core MVC Basics", Degree = 100, MinDegree = 60, Hours = 45, Dept_Id = 1 },
                new Course { Id = 3, Name = "Introduction to Python & AI", Degree = 100, MinDegree = 50, Hours = 40, Dept_Id = 3 },
                new Course { Id = 4, Name = "Linux System Administration", Degree = 100, MinDegree = 50, Hours = 30, Dept_Id = 2 },
                new Course { Id = 5, Name = "Data Analytics & Visualization", Degree = 100, MinDegree = 50, Hours = 50, Dept_Id = 5 },
                new Course { Id = 6, Name = "Ethical Hacking & Penetration Testing", Degree = 100, MinDegree = 60, Hours = 55, Dept_Id = 4 },
                new Course { Id = 7, Name = "Cloud Computing with Azure", Degree = 100, MinDegree = 50, Hours = 40, Dept_Id = 6 },
                new Course { Id = 8, Name = "Deep Learning & Neural Networks", Degree = 100, MinDegree = 60, Hours = 60, Dept_Id = 3 },
                new Course { Id = 9, Name = "Advanced React & Next.js Framework", Degree = 100, MinDegree = 50, Hours = 48, Dept_Id = 1 },
                new Course { Id = 10, Name = "Introduction to Database Systems", Degree = 100, MinDegree = 50, Hours = 36, Dept_Id = 1 }
            );

            // Seed Instructors
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, Name = "Ahmed Mansour", Salary = 15000, Address = "Cairo, Egypt", Dept_Id = 1, Crs_Id = 1, Image = "ahmed.png" },
                new Instructor { Id = 2, Name = "Nouran Ezzat", Salary = 18000, Address = "Alexandria, Egypt", Dept_Id = 1, Crs_Id = 2, Image = "nouran.png" },
                new Instructor { Id = 3, Name = "Sarah Mahmoud", Salary = 20000, Address = "Giza, Egypt", Dept_Id = 3, Crs_Id = 3, Image = "sarah.png" },
                new Instructor { Id = 4, Name = "Kareem Fouad", Salary = 22000, Address = "Maadi, Cairo", Dept_Id = 4, Crs_Id = 6, Image = "kareem.png" },
                new Instructor { Id = 5, Name = "Laila Hassan", Salary = 21000, Address = "Zamalek, Cairo", Dept_Id = 5, Crs_Id = 5, Image = "laila.png" },
                new Instructor { Id = 6, Name = "Mostafa Kamel", Salary = 19000, Address = "Nasr City, Cairo", Dept_Id = 6, Crs_Id = 7, Image = "mostafa.png" },
                new Instructor { Id = 7, Name = "Sherif Anwar", Salary = 24000, Address = "Giza, Egypt", Dept_Id = 3, Crs_Id = 8, Image = "sherif.png" },
                new Instructor { Id = 8, Name = "Yasmin Soliman", Salary = 17500, Address = "Alexandria, Egypt", Dept_Id = 1, Crs_Id = 9, Image = "yasmin.png" },
                new Instructor { Id = 9, Name = "Tarek Nour", Salary = 16000, Address = "Heliopolis, Cairo", Dept_Id = 1, Crs_Id = 10, Image = "tarek.png" }
            );

            // Seed Trainees
            modelBuilder.Entity<Trainee>().HasData(
                new Trainee { Id = 1, Name = "Omar Hassan", Address = "Cairo, Egypt", Grade = "Excellent", Dept_Id = 1, Image = "omar.png" },
                new Trainee { Id = 2, Name = "Mariam Youssef", Address = "Alexandria, Egypt", Grade = "Very Good", Dept_Id = 1, Image = "mariam.png" },
                new Trainee { Id = 3, Name = "Ziad Tarek", Address = "Giza, Egypt", Grade = "Excellent", Dept_Id = 3, Image = "ziad.png" },
                new Trainee { Id = 4, Name = "Youssef Mohamed", Address = "Giza, Egypt", Grade = "Excellent", Dept_Id = 1, Image = "youssef.png" },
                new Trainee { Id = 5, Name = "Farida Amr", Address = "Zamalek, Cairo", Grade = "Very Good", Dept_Id = 5, Image = "farida.png" },
                new Trainee { Id = 6, Name = "Hassan Soliman", Address = "Maadi, Cairo", Grade = "Good", Dept_Id = 4, Image = "hassan.png" },
                new Trainee { Id = 7, Name = "Salma Khaled", Address = "Heliopolis, Cairo", Grade = "Excellent", Dept_Id = 3, Image = "salma.png" },
                new Trainee { Id = 8, Name = "Nader Ibrahim", Address = "Alexandria, Egypt", Grade = "Pass", Dept_Id = 6, Image = "nader.png" },
                new Trainee { Id = 9, Name = "Dina Yasser", Address = "Tanta, Egypt", Grade = "Very Good", Dept_Id = 1, Image = "dina.png" },
                new Trainee { Id = 10, Name = "Karim Walid", Address = "Mansoura, Egypt", Grade = "Good", Dept_Id = 4, Image = "karim.png" },
                new Trainee { Id = 11, Name = "Nourhan Amr", Address = "Sheraton, Cairo", Grade = "Excellent", Dept_Id = 5, Image = "nourhan.png" },
                new Trainee { Id = 12, Name = "Hady Ahmed", Address = "Rehab, Cairo", Grade = "Pass", Dept_Id = 6, Image = "hady.png" }
            );

            // Seed CourseResults
            modelBuilder.Entity<CourseResult>().HasData(
                new CourseResult { Id = 1, Degree = 95, Crs_Id = 1, Trainee_Id = 1 },
                new CourseResult { Id = 2, Degree = 88, Crs_Id = 2, Trainee_Id = 1 },
                new CourseResult { Id = 3, Degree = 55, Crs_Id = 1, Trainee_Id = 2 },
                new CourseResult { Id = 4, Degree = 48, Crs_Id = 2, Trainee_Id = 2 }, // Fail (Passing is 60)
                new CourseResult { Id = 5, Degree = 92, Crs_Id = 3, Trainee_Id = 3 },
                new CourseResult { Id = 6, Degree = 98, Crs_Id = 9, Trainee_Id = 4 },
                new CourseResult { Id = 7, Degree = 95, Crs_Id = 10, Trainee_Id = 4 },
                new CourseResult { Id = 8, Degree = 88, Crs_Id = 5, Trainee_Id = 5 },
                new CourseResult { Id = 9, Degree = 72, Crs_Id = 6, Trainee_Id = 6 },
                new CourseResult { Id = 10, Degree = 94, Crs_Id = 3, Trainee_Id = 7 },
                new CourseResult { Id = 11, Degree = 89, Crs_Id = 8, Trainee_Id = 7 },
                new CourseResult { Id = 12, Degree = 64, Crs_Id = 7, Trainee_Id = 8 },
                new CourseResult { Id = 13, Degree = 82, Crs_Id = 1, Trainee_Id = 9 },
                new CourseResult { Id = 14, Degree = 78, Crs_Id = 2, Trainee_Id = 9 },
                new CourseResult { Id = 15, Degree = 85, Crs_Id = 9, Trainee_Id = 9 },
                new CourseResult { Id = 16, Degree = 58, Crs_Id = 6, Trainee_Id = 10 }, // Fail
                new CourseResult { Id = 17, Degree = 91, Crs_Id = 5, Trainee_Id = 11 },
                new CourseResult { Id = 18, Degree = 60, Crs_Id = 7, Trainee_Id = 12 }
            );
        }
    }
}
