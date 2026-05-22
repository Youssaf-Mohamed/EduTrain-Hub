using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Windows_Programing.Migrations
{
    /// <inheritdoc />
    public partial class ExpandSeedDataAndPremiumStyling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Degree", "Dept_Id", "Hours", "MinDegree", "Name" },
                values: new object[,]
                {
                    { 8, 100, 3, 60, 60, "Deep Learning & Neural Networks" },
                    { 9, 100, 1, 48, 50, "Advanced React & Next.js Framework" },
                    { 10, 100, 1, 36, 50, "Introduction to Database Systems" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Manager", "Name" },
                values: new object[,]
                {
                    { 4, "Dr. Kareem Fouad", "Cyber Security (CS)" },
                    { 5, "Dr. Laila Hassan", "Data Science (DS)" },
                    { 6, "Dr. Mostafa Kamel", "Cloud Computing & DevOps" }
                });

            migrationBuilder.InsertData(
                table: "Trainees",
                columns: new[] { "Id", "Address", "Dept_Id", "Grade", "Image", "Name" },
                values: new object[,]
                {
                    { 4, "Giza, Egypt", 1, "Excellent", "youssef.png", "Youssef Mohamed" },
                    { 7, "Heliopolis, Cairo", 3, "Excellent", "salma.png", "Salma Khaled" },
                    { 9, "Tanta, Egypt", 1, "Very Good", "dina.png", "Dina Yasser" }
                });

            migrationBuilder.InsertData(
                table: "CourseResults",
                columns: new[] { "Id", "Crs_Id", "Degree", "Trainee_Id" },
                values: new object[,]
                {
                    { 6, 9, 98, 4 },
                    { 7, 10, 95, 4 },
                    { 10, 3, 94, 7 },
                    { 11, 8, 89, 7 },
                    { 13, 1, 82, 9 },
                    { 14, 2, 78, 9 },
                    { 15, 9, 85, 9 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Degree", "Dept_Id", "Hours", "MinDegree", "Name" },
                values: new object[,]
                {
                    { 5, 100, 5, 50, 50, "Data Analytics & Visualization" },
                    { 6, 100, 4, 55, 60, "Ethical Hacking & Penetration Testing" },
                    { 7, 100, 6, 40, 50, "Cloud Computing with Azure" }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Address", "Crs_Id", "Dept_Id", "Image", "Name", "Salary" },
                values: new object[,]
                {
                    { 7, "Giza, Egypt", 8, 3, "sherif.png", "Sherif Anwar", 24000.0 },
                    { 8, "Alexandria, Egypt", 9, 1, "yasmin.png", "Yasmin Soliman", 17500.0 },
                    { 9, "Heliopolis, Cairo", 10, 1, "tarek.png", "Tarek Nour", 16000.0 }
                });

            migrationBuilder.InsertData(
                table: "Trainees",
                columns: new[] { "Id", "Address", "Dept_Id", "Grade", "Image", "Name" },
                values: new object[,]
                {
                    { 5, "Zamalek, Cairo", 5, "Very Good", "farida.png", "Farida Amr" },
                    { 6, "Maadi, Cairo", 4, "Good", "hassan.png", "Hassan Soliman" },
                    { 8, "Alexandria, Egypt", 6, "Pass", "nader.png", "Nader Ibrahim" },
                    { 10, "Mansoura, Egypt", 4, "Good", "karim.png", "Karim Walid" },
                    { 11, "Sheraton, Cairo", 5, "Excellent", "nourhan.png", "Nourhan Amr" },
                    { 12, "Rehab, Cairo", 6, "Pass", "hady.png", "Hady Ahmed" }
                });

            migrationBuilder.InsertData(
                table: "CourseResults",
                columns: new[] { "Id", "Crs_Id", "Degree", "Trainee_Id" },
                values: new object[,]
                {
                    { 8, 5, 88, 5 },
                    { 9, 6, 72, 6 },
                    { 12, 7, 64, 8 },
                    { 16, 6, 58, 10 },
                    { 17, 5, 91, 11 },
                    { 18, 7, 60, 12 }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Address", "Crs_Id", "Dept_Id", "Image", "Name", "Salary" },
                values: new object[,]
                {
                    { 4, "Maadi, Cairo", 6, 4, "kareem.png", "Kareem Fouad", 22000.0 },
                    { 5, "Zamalek, Cairo", 5, 5, "laila.png", "Laila Hassan", 21000.0 },
                    { 6, "Nasr City, Cairo", 7, 6, "mostafa.png", "Mostafa Kamel", 19000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CourseResults",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Trainees",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
