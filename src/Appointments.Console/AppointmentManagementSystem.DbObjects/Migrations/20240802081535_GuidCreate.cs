using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentManagementSystem.DbObjects.Migrations
{
    /// <inheritdoc />
    public partial class GuidCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MassageServices",
                columns: table => new
                {
                    MassageServiceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MassageServices", x => x.MassageServiceId);
                });

            migrationBuilder.CreateTable(
                name: "MasseusePreference",
                columns: table => new
                {
                    PreferenceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasseusePreference", x => x.PreferenceId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceType",
                columns: table => new
                {
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceType", x => x.ServiceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingDuration",
                columns: table => new
                {
                    TrainingDurationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDuration", x => x.TrainingDurationId);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_Appointment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MassageAppointment",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MassageServices = table.Column<int>(type: "int", nullable: false),
                    Preference = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MassageAppointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_MassageAppointment_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalTrainingAppointment",
                columns: table => new
                {
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingDuration = table.Column<int>(type: "int", nullable: true),
                    CustomerComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InjuriesOrPains = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalTrainingAppointment", x => x.AppointmentId);
                    table.ForeignKey(
                        name: "FK_PersonalTrainingAppointment_Appointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointment",
                        principalColumn: "AppointmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MassageServices",
                columns: new[] { "MassageServiceId", "Name" },
                values: new object[,]
                {
                    { 0, "RelaxingMassage" },
                    { 1, "HotStoneTherapy" },
                    { 2, "Reflexology" }
                });

            migrationBuilder.InsertData(
                table: "MasseusePreference",
                columns: new[] { "PreferenceId", "Name" },
                values: new object[,]
                {
                    { 0, "Male" },
                    { 1, "Female" }
                });

            migrationBuilder.InsertData(
                table: "ServiceType",
                columns: new[] { "ServiceTypeId", "Name" },
                values: new object[,]
                {
                    { 0, "Massage" },
                    { 1, "PersonalTraining" }
                });

            migrationBuilder.InsertData(
                table: "TrainingDuration",
                columns: new[] { "TrainingDurationId", "Name" },
                values: new object[,]
                {
                    { 0, "ThirtyMinutes" },
                    { 1, "OneHour" },
                    { 2, "OneHourThirtyMinutes" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_CustomerId",
                table: "Appointment",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MassageAppointment");

            migrationBuilder.DropTable(
                name: "MassageServices");

            migrationBuilder.DropTable(
                name: "MasseusePreference");

            migrationBuilder.DropTable(
                name: "PersonalTrainingAppointment");

            migrationBuilder.DropTable(
                name: "ServiceType");

            migrationBuilder.DropTable(
                name: "TrainingDuration");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
