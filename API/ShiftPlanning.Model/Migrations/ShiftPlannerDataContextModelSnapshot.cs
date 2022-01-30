﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShiftPlanning.Model;

namespace ShiftPlanning.Model.Migrations
{
    [DbContext(typeof(ShiftPlannerDataContext))]
    partial class ShiftPlannerDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("prod")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Role_Id")
                        .HasColumnType("int");

                    b.HasKey("Employee_Id", "Role_Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("RoleEmployees");
                });

            modelBuilder.Entity("EmployeeShift", b =>
                {
                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Shift_Id")
                        .HasColumnType("int");

                    b.HasKey("Employee_Id", "Shift_Id");

                    b.HasIndex("Shift_Id");

                    b.ToTable("ShiftEmployees");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.CheckIn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Shift_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("Shift_Id");

                    b.ToTable("CheckIns");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.EmailSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailHost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("EmailSettings");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Organization_Id")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PodioId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WantShifts")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Organization_Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.EmployeeAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<int?>("ScheduledShift_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("ScheduledShift_Id");

                    b.ToTable("EmployeeAssignment");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.EmployeeTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Organization_Id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Organization_Id");

                    b.ToTable("EmployeeTitle");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Friendship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Friend_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Employee_Id");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmailSettings_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmailSettings_Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Employee_Id")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("ScheduledShift_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Employee_Id");

                    b.HasIndex("ScheduledShift_Id");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfWeeks")
                        .HasColumnType("int");

                    b.Property<int?>("Organization_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Organization_Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.ScheduledShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("time");

                    b.Property<int>("MaxOnShift")
                        .HasColumnType("int");

                    b.Property<int>("MinOnShift")
                        .HasColumnType("int");

                    b.Property<int?>("Schedule_Id")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("Schedule_Id");

                    b.ToTable("ScheduledShifts");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Organization_Id")
                        .HasColumnType("int");

                    b.Property<int?>("Schedule_Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Organization_Id");

                    b.HasIndex("Schedule_Id");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("TokenHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("EmployeeRole", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("Employee_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShiftPlanning.Model.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeShift", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("Employee_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShiftPlanning.Model.Models.Shift", null)
                        .WithMany()
                        .HasForeignKey("Shift_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.CheckIn", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", "Employee")
                        .WithMany("CheckIns")
                        .HasForeignKey("Employee_Id");

                    b.HasOne("ShiftPlanning.Model.Models.Shift", "Shift")
                        .WithMany("CheckIns")
                        .HasForeignKey("Shift_Id");

                    b.Navigation("Employee");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Employee", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Organization", "Organization")
                        .WithMany("Employees")
                        .HasForeignKey("Organization_Id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.EmployeeAssignment", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", "Employee")
                        .WithMany("EmployeeAssignments")
                        .HasForeignKey("Employee_Id");

                    b.HasOne("ShiftPlanning.Model.Models.ScheduledShift", "ScheduledShift")
                        .WithMany("EmployeeAssignments")
                        .HasForeignKey("ScheduledShift_Id");

                    b.Navigation("Employee");

                    b.Navigation("ScheduledShift");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.EmployeeTitle", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Organization", "Organization")
                        .WithMany("EmployeeTitles")
                        .HasForeignKey("Organization_Id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Friendship", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", "Employee")
                        .WithMany("Friendships")
                        .HasForeignKey("Employee_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Organization", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.EmailSettings", "EmailSettings")
                        .WithMany()
                        .HasForeignKey("EmailSettings_Id");

                    b.Navigation("EmailSettings");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Photo", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Organization", "Organization")
                        .WithMany("Photos")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Preference", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", "Employee")
                        .WithMany("Preferences")
                        .HasForeignKey("Employee_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShiftPlanning.Model.Models.ScheduledShift", "ScheduledShift")
                        .WithMany("Preferences")
                        .HasForeignKey("ScheduledShift_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("ScheduledShift");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Schedule", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Organization", "Organization")
                        .WithMany("Schedules")
                        .HasForeignKey("Organization_Id");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.ScheduledShift", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Schedule", "Schedule")
                        .WithMany("ScheduledShifts")
                        .HasForeignKey("Schedule_Id");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Shift", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Organization", "Organization")
                        .WithMany("Shifts")
                        .HasForeignKey("Organization_Id");

                    b.HasOne("ShiftPlanning.Model.Models.Schedule", "Schedule")
                        .WithMany("Shifts")
                        .HasForeignKey("Schedule_Id");

                    b.Navigation("Organization");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Token", b =>
                {
                    b.HasOne("ShiftPlanning.Model.Models.Employee", null)
                        .WithMany("Tokens")
                        .HasForeignKey("EmployeeId");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Employee", b =>
                {
                    b.Navigation("CheckIns");

                    b.Navigation("EmployeeAssignments");

                    b.Navigation("Friendships");

                    b.Navigation("Preferences");

                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Organization", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("EmployeeTitles");

                    b.Navigation("Photos");

                    b.Navigation("Schedules");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Schedule", b =>
                {
                    b.Navigation("ScheduledShifts");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.ScheduledShift", b =>
                {
                    b.Navigation("EmployeeAssignments");

                    b.Navigation("Preferences");
                });

            modelBuilder.Entity("ShiftPlanning.Model.Models.Shift", b =>
                {
                    b.Navigation("CheckIns");
                });
#pragma warning restore 612, 618
        }
    }
}
