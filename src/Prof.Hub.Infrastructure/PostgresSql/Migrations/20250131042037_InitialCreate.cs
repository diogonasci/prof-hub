using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prof.Hub.Infrastructure.PostgresSql.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    state = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    parent_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_phone_number = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    parent_created_by = table.Column<string>(type: "text", nullable: true),
                    parent_last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    parent_last_modified_by = table.Column<string>(type: "text", nullable: true),
                    class_hours = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    street = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    state = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    postal_code = table.Column<string>(type: "text", nullable: false),
                    hourly_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "group_lessons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_group_lessons_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "private_lessons",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_private_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_private_lessons_student_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_private_lessons_teacher_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "group_lesson_students",
                columns: table => new
                {
                    group_lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_lesson_students", x => new { x.group_lesson_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_GroupLessonStudent_GroupLesson",
                        column: x => x.group_lesson_id,
                        principalTable: "group_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLessonStudent_Student",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_group_lesson_students_student_id",
                table: "group_lesson_students",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_group_lessons_teacher_id",
                table: "group_lessons",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "ix_private_lessons_student_id",
                table: "private_lessons",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "ix_private_lessons_teacher_id",
                table: "private_lessons",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_lesson_students");

            migrationBuilder.DropTable(
                name: "private_lessons");

            migrationBuilder.DropTable(
                name: "group_lessons");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
