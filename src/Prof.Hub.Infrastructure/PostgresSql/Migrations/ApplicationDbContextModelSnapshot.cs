﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Prof.Hub.Infrastructure.PostgresSql;

#nullable disable

namespace Prof.Hub.Infrastructure.PostgresSql.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GroupLessonStudent", b =>
                {
                    b.Property<Guid>("GroupLessonId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_lesson_id");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.HasKey("GroupLessonId", "StudentId")
                        .HasName("pk_group_lesson_students");

                    b.HasIndex("StudentId")
                        .HasDatabaseName("ix_group_lesson_students_student_id");

                    b.ToTable("group_lesson_students", (string)null);
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.GroupLesson.GroupLesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_modified_by");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id")
                        .HasName("pk_group_lessons");

                    b.HasIndex("TeacherId")
                        .HasDatabaseName("ix_group_lessons_teacher_id");

                    b.ToTable("group_lessons", (string)null);
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.PrivateLesson.PrivateLesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_modified_by");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.Property<Guid>("TeacherId")
                        .HasColumnType("uuid")
                        .HasColumnName("teacher_id");

                    b.HasKey("Id")
                        .HasName("pk_private_lessons");

                    b.HasIndex("StudentId")
                        .HasDatabaseName("ix_private_lessons_student_id");

                    b.HasIndex("TeacherId")
                        .HasDatabaseName("ix_private_lessons_teacher_id");

                    b.ToTable("private_lessons", (string)null);
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Student.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("created_by");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.HasKey("Id")
                        .HasName("pk_students");

                    b.ToTable("students", (string)null);
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Teacher.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("created_by");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("numeric")
                        .HasColumnName("hourly_rate");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.HasKey("Id")
                        .HasName("pk_teachers");

                    b.ToTable("teachers", (string)null);
                });

            modelBuilder.Entity("GroupLessonStudent", b =>
                {
                    b.HasOne("Prof.Hub.Domain.Aggregates.GroupLesson.GroupLesson", null)
                        .WithMany()
                        .HasForeignKey("GroupLessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_GroupLessonStudent_GroupLesson");

                    b.HasOne("Prof.Hub.Domain.Aggregates.Student.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_GroupLessonStudent_Student");
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.GroupLesson.GroupLesson", b =>
                {
                    b.HasOne("Prof.Hub.Domain.Aggregates.Teacher.Teacher", null)
                        .WithMany("GroupLessons")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_group_lessons_teacher_teacher_id");
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.PrivateLesson.PrivateLesson", b =>
                {
                    b.HasOne("Prof.Hub.Domain.Aggregates.Student.Student", null)
                        .WithMany("PrivateLessons")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_private_lessons_student_student_id");

                    b.HasOne("Prof.Hub.Domain.Aggregates.Teacher.Teacher", null)
                        .WithMany("PrivateLessons")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_private_lessons_teacher_teacher_id");
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Student.Student", b =>
                {
                    b.OwnsOne("Prof.Hub.Domain.Aggregates.Student.Parent", "Parent", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("parent_created");

                            b1.Property<string>("CreatedBy")
                                .HasColumnType("text")
                                .HasColumnName("parent_created_by");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("parent_email");

                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("parent_id");

                            b1.Property<DateTime?>("LastModified")
                                .HasColumnType("timestamp with time zone")
                                .HasColumnName("parent_last_modified");

                            b1.Property<string>("LastModifiedBy")
                                .HasColumnType("text")
                                .HasColumnName("parent_last_modified_by");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("parent_name");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("parent_phone_number");

                            b1.HasKey("StudentId");

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId")
                                .HasConstraintName("fk_students_students_id");
                        });

                    b.OwnsOne("Prof.Hub.Domain.Aggregates.Common.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("city");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("postal_code");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("street");

                            b1.HasKey("StudentId");

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId")
                                .HasConstraintName("fk_students_students_id");
                        });

                    b.OwnsOne("Prof.Hub.Domain.Aggregates.Student.ValueObjects.ClassHours", "ClassHours", b1 =>
                        {
                            b1.Property<Guid>("StudentId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("class_hours");

                            b1.HasKey("StudentId");

                            b1.ToTable("students");

                            b1.WithOwner()
                                .HasForeignKey("StudentId")
                                .HasConstraintName("fk_students_students_id");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("ClassHours")
                        .IsRequired();

                    b.Navigation("Parent")
                        .IsRequired();
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Teacher.Teacher", b =>
                {
                    b.OwnsOne("Prof.Hub.Domain.Aggregates.Common.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("TeacherId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("city");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("postal_code");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("street");

                            b1.HasKey("TeacherId");

                            b1.ToTable("teachers");

                            b1.WithOwner()
                                .HasForeignKey("TeacherId")
                                .HasConstraintName("fk_teachers_teachers_id");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Student.Student", b =>
                {
                    b.Navigation("PrivateLessons");
                });

            modelBuilder.Entity("Prof.Hub.Domain.Aggregates.Teacher.Teacher", b =>
                {
                    b.Navigation("GroupLessons");

                    b.Navigation("PrivateLessons");
                });
#pragma warning restore 612, 618
        }
    }
}
