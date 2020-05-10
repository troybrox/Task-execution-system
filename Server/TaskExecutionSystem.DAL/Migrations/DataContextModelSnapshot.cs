﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskExecutionSystem.DAL.Data;

namespace TaskExecutionSystem.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("IdentityRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.RepoFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileURI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepositoryModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryModelId")
                        .IsUnique();

                    b.ToTable("RepoFiles");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.SolutionFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileURI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SolutionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SolutionId")
                        .IsUnique();

                    b.ToTable("SolutionFiles");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.TaskFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileURI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskModelId")
                        .IsUnique();

                    b.ToTable("TaskFiles");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<int>("NotificationCounter")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId");

                    b.HasIndex("GroupId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<int>("NotificationCounter")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(50);

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasAlternateKey("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Registration.StudentRegisterRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("StudentRegisterRequests");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Registration.TeacherRegisterRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("TeacherRegisterRequests");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Relations.GroupTeacherSubjectItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("GroupTeacherSubjectItems");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Relations.TaskStudentItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskStudentItems");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.Paragraph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("ThemeId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.RepositoryModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("RepositoryModels");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.HasIndex("FacultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("NumberName")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.HasKey("Id");

                    b.HasAlternateKey("NumberName");

                    b.HasIndex("FacultyId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Task.Solution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("InTime")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TaskId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Task.TaskModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("DATETIME");

                    b.Property<string>("ContentText")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<DateTime>("FinishDate")
                        .HasColumnType("DATETIME");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("DATETIME");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.HasIndex("TypeId");

                    b.ToTable("TaskModels");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Task.TypeOfTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("TaskTypes");
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.RepoFile", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Repository.RepositoryModel", "RepositoryModel")
                        .WithOne("File")
                        .HasForeignKey("TaskExecutionSystem.DAL.Entities.File.RepoFile", "RepositoryModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.SolutionFile", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Task.Solution", "Solution")
                        .WithOne("File")
                        .HasForeignKey("TaskExecutionSystem.DAL.Entities.File.SolutionFile", "SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.File.TaskFile", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Task.TaskModel", "TaskModel")
                        .WithOne("File")
                        .HasForeignKey("TaskExecutionSystem.DAL.Entities.File.TaskFile", "TaskModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.Student", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("TaskExecutionSystem.DAL.Entities.Identity.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Identity.Teacher", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Department", "Department")
                        .WithMany("Teachers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.User", "User")
                        .WithOne("Teacher")
                        .HasForeignKey("TaskExecutionSystem.DAL.Entities.Identity.Teacher", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Registration.StudentRegisterRequest", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Group", "Group")
                        .WithMany("StudentRegisterRequests")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Registration.TeacherRegisterRequest", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Department", "Department")
                        .WithMany("TeacherRegisterRequests")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Relations.GroupTeacherSubjectItem", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Group", "Group")
                        .WithMany("GroupTeacherSubjectItems")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Subject", "Subject")
                        .WithMany("GroupTeacherSubjectItems")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.Teacher", "Teacher")
                        .WithMany("GroupTeacherSubjectItems")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Relations.TaskStudentItem", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.Student", "Student")
                        .WithMany("TaskStudentItems")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Task.TaskModel", "Task")
                        .WithMany("TaskStudentItems")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.Paragraph", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.File.RepoFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Repository.Theme", "Theme")
                        .WithMany("Paragraphs")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.RepositoryModel", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Subject", "Subject")
                        .WithMany("Repositories")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Repository.Theme", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.File.RepoFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Repository.RepositoryModel", "Repository")
                        .WithMany("Themes")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Department", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Faculty", "Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Studies.Group", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Faculty", "Faculty")
                        .WithMany("Groups")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Task.Solution", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.Student", "Student")
                        .WithMany("Solutions")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Task.TaskModel", "TaskModel")
                        .WithMany("Solutions")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskExecutionSystem.DAL.Entities.Task.TaskModel", b =>
                {
                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Group", "Group")
                        .WithMany("Tasks")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Studies.Subject", "Subject")
                        .WithMany("Tasks")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Identity.Teacher", "Teacher")
                        .WithMany("Tasks")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskExecutionSystem.DAL.Entities.Task.TypeOfTask", "Type")
                        .WithMany("Tasks")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
