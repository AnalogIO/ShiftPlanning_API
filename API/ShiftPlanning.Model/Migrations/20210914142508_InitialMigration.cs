using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShiftPlanning.Model.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "prod");

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailHost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailSettings_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_EmailSettings_EmailSettings_Id",
                        column: x => x.EmailSettings_Id,
                        principalSchema: "prod",
                        principalTable: "EmailSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    WantShifts = table.Column<int>(type: "int", nullable: false),
                    Organization_Id = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PodioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Organizations_Organization_Id",
                        column: x => x.Organization_Id,
                        principalSchema: "prod",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTitle",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Organization_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTitle_Organizations_Organization_Id",
                        column: x => x.Organization_Id,
                        principalSchema: "prod",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "prod",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfWeeks = table.Column<int>(type: "int", nullable: false),
                    Organization_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Organizations_Organization_Id",
                        column: x => x.Organization_Id,
                        principalSchema: "prod",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Friend_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleEmployees",
                schema: "prod",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleEmployees", x => new { x.Employee_Id, x.Role_Id });
                    table.ForeignKey(
                        name: "FK_RoleEmployees_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleEmployees_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalSchema: "prod",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledShifts",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxOnShift = table.Column<int>(type: "int", nullable: false),
                    MinOnShift = table.Column<int>(type: "int", nullable: false),
                    Schedule_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledShifts_Schedules_Schedule_Id",
                        column: x => x.Schedule_Id,
                        principalSchema: "prod",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Organization_Id = table.Column<int>(type: "int", nullable: true),
                    Schedule_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_Organizations_Organization_Id",
                        column: x => x.Organization_Id,
                        principalSchema: "prod",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shifts_Schedules_Schedule_Id",
                        column: x => x.Schedule_Id,
                        principalSchema: "prod",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAssignment",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Employee_Id = table.Column<int>(type: "int", nullable: true),
                    ScheduledShift_Id = table.Column<int>(type: "int", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignment_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignment_ScheduledShifts_ScheduledShift_Id",
                        column: x => x.ScheduledShift_Id,
                        principalSchema: "prod",
                        principalTable: "ScheduledShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    ScheduledShift_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Preferences_ScheduledShifts_ScheduledShift_Id",
                        column: x => x.ScheduledShift_Id,
                        principalSchema: "prod",
                        principalTable: "ScheduledShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                schema: "prod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Employee_Id = table.Column<int>(type: "int", nullable: true),
                    Shift_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckIns_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckIns_Shifts_Shift_Id",
                        column: x => x.Shift_Id,
                        principalSchema: "prod",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShiftEmployees",
                schema: "prod",
                columns: table => new
                {
                    Employee_Id = table.Column<int>(type: "int", nullable: false),
                    Shift_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftEmployees", x => new { x.Employee_Id, x.Shift_Id });
                    table.ForeignKey(
                        name: "FK_ShiftEmployees_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "prod",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftEmployees_Shifts_Shift_Id",
                        column: x => x.Shift_Id,
                        principalSchema: "prod",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_Employee_Id",
                schema: "prod",
                table: "CheckIns",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_Shift_Id",
                schema: "prod",
                table: "CheckIns",
                column: "Shift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignment_Employee_Id",
                schema: "prod",
                table: "EmployeeAssignment",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignment_ScheduledShift_Id",
                schema: "prod",
                table: "EmployeeAssignment",
                column: "ScheduledShift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Organization_Id",
                schema: "prod",
                table: "Employees",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTitle_Organization_Id",
                schema: "prod",
                table: "EmployeeTitle",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_Employee_Id",
                schema: "prod",
                table: "Friendships",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EmailSettings_Id",
                schema: "prod",
                table: "Organizations",
                column: "EmailSettings_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_OrganizationId",
                schema: "prod",
                table: "Photos",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_Employee_Id",
                schema: "prod",
                table: "Preferences",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_ScheduledShift_Id",
                schema: "prod",
                table: "Preferences",
                column: "ScheduledShift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleEmployees_Role_Id",
                schema: "prod",
                table: "RoleEmployees",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledShifts_Schedule_Id",
                schema: "prod",
                table: "ScheduledShifts",
                column: "Schedule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_Organization_Id",
                schema: "prod",
                table: "Schedules",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftEmployees_Shift_Id",
                schema: "prod",
                table: "ShiftEmployees",
                column: "Shift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Organization_Id",
                schema: "prod",
                table: "Shifts",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Schedule_Id",
                schema: "prod",
                table: "Shifts",
                column: "Schedule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Token_EmployeeId",
                schema: "prod",
                table: "Token",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "EmployeeAssignment",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "EmployeeTitle",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Friendships",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "RoleEmployees",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "ShiftEmployees",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Token",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "ScheduledShifts",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "prod");

            migrationBuilder.DropTable(
                name: "EmailSettings",
                schema: "prod");
        }
    }
}
