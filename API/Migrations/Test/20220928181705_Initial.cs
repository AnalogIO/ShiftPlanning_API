using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftPlanning.Model.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "test");

            migrationBuilder.CreateTable(
                name: "EmailSettings",
                schema: "test",
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
                schema: "test",
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
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "EmailSettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTitle",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Organization_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Organizations_Organization_Id",
                        column: x => x.Organization_Id,
                        principalSchema: "test",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleEmployees",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleEmployees_Roles_Role_Id",
                        column: x => x.Role_Id,
                        principalSchema: "test",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                schema: "test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employee_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScheduledShifts",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Schedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shifts_Schedules_Schedule_Id",
                        column: x => x.Schedule_Id,
                        principalSchema: "test",
                        principalTable: "Schedules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAssignments",
                schema: "test",
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
                    table.PrimaryKey("PK_EmployeeAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_Employees_Employee_Id",
                        column: x => x.Employee_Id,
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAssignments_ScheduledShifts_ScheduledShift_Id",
                        column: x => x.ScheduledShift_Id,
                        principalSchema: "test",
                        principalTable: "ScheduledShifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Preferences_ScheduledShifts_ScheduledShift_Id",
                        column: x => x.ScheduledShift_Id,
                        principalSchema: "test",
                        principalTable: "ScheduledShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckIns_Shifts_Shift_Id",
                        column: x => x.Shift_Id,
                        principalSchema: "test",
                        principalTable: "Shifts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShiftEmployees",
                schema: "test",
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
                        principalSchema: "test",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftEmployees_Shifts_Shift_Id",
                        column: x => x.Shift_Id,
                        principalSchema: "test",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_Employee_Id",
                schema: "test",
                table: "CheckIns",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_Shift_Id",
                schema: "test",
                table: "CheckIns",
                column: "Shift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_Employee_Id",
                schema: "test",
                table: "EmployeeAssignments",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAssignments_ScheduledShift_Id",
                schema: "test",
                table: "EmployeeAssignments",
                column: "ScheduledShift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Organization_Id",
                schema: "test",
                table: "Employees",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTitle_Organization_Id",
                schema: "test",
                table: "EmployeeTitle",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_Employee_Id",
                schema: "test",
                table: "Friendships",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_EmailSettings_Id",
                schema: "test",
                table: "Organizations",
                column: "EmailSettings_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Organization_Id",
                schema: "test",
                table: "Photos",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_Employee_Id",
                schema: "test",
                table: "Preferences",
                column: "Employee_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_ScheduledShift_Id",
                schema: "test",
                table: "Preferences",
                column: "ScheduledShift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleEmployees_Role_Id",
                schema: "test",
                table: "RoleEmployees",
                column: "Role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledShifts_Schedule_Id",
                schema: "test",
                table: "ScheduledShifts",
                column: "Schedule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_Organization_Id",
                schema: "test",
                table: "Schedules",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftEmployees_Shift_Id",
                schema: "test",
                table: "ShiftEmployees",
                column: "Shift_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Organization_Id",
                schema: "test",
                table: "Shifts",
                column: "Organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_Schedule_Id",
                schema: "test",
                table: "Shifts",
                column: "Schedule_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_Employee_Id",
                schema: "test",
                table: "Tokens",
                column: "Employee_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns",
                schema: "test");

            migrationBuilder.DropTable(
                name: "EmployeeAssignments",
                schema: "test");

            migrationBuilder.DropTable(
                name: "EmployeeTitle",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Friendships",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Preferences",
                schema: "test");

            migrationBuilder.DropTable(
                name: "RoleEmployees",
                schema: "test");

            migrationBuilder.DropTable(
                name: "ShiftEmployees",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Tokens",
                schema: "test");

            migrationBuilder.DropTable(
                name: "ScheduledShifts",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "test");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "test");

            migrationBuilder.DropTable(
                name: "EmailSettings",
                schema: "test");
        }
    }
}
