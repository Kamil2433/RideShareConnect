using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RideShareConnect.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverProfiles",
                columns: table => new
                {
                    DriverProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LicenseExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverProfiles", x => x.DriverProfileId);
                });

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     columns: table => new
            //     {
            //         UserId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //         PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //         Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         IsEmailVerified = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.UserId);
            //     });

            migrationBuilder.CreateTable(
                name: "DriverRatings",
                columns: table => new
                {
                    RatingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    RideId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRatings", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_DriverRatings_DriverProfiles_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DriverProfiles",
                        principalColumn: "DriverProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SeatingCapacity = table.Column<int>(type: "int", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_DriverProfiles_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DriverProfiles",
                        principalColumn: "DriverProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            // migrationBuilder.CreateTable(
            //     name: "TwoFactorCodes",
            //     columns: table => new
            //     {
            //         CodeId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         IsUsed = table.Column<bool>(type: "bit", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_TwoFactorCodes", x => x.CodeId);
            //         table.ForeignKey(
            //             name: "FK_TwoFactorCodes_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "UserId",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UserProfiles",
            //     columns: table => new
            //     {
            //         ProfileId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //         LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //         PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //         Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //         ProfilePicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserProfiles", x => x.ProfileId);
            //         table.ForeignKey(
            //             name: "FK_UserProfiles_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "UserId",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateTable(
            //     name: "UserSettings",
            //     columns: table => new
            //     {
            //         SettingsId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         TermsAccepted = table.Column<bool>(type: "bit", nullable: false),
            //         EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
            //         SMSNotifications = table.Column<bool>(type: "bit", nullable: false),
            //         PushNotifications = table.Column<bool>(type: "bit", nullable: false),
            //         Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //         Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
            //         TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_UserSettings", x => x.SettingsId);
            //         table.ForeignKey(
            //             name: "FK_UserSettings_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "UserId",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDocuments",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDocuments", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_VehicleDocuments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverRatings_DriverId",
                table: "DriverRatings",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_VehicleId",
                table: "MaintenanceRecords",
                column: "VehicleId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_TwoFactorCodes_UserId",
            //     table: "TwoFactorCodes",
            //     column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserProfiles_UserId",
            //     table: "UserProfiles",
            //     column: "UserId",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_Users_Email",
            //     table: "Users",
            //     column: "Email",
            //     unique: true);

            // migrationBuilder.CreateIndex(
            //     name: "IX_UserSettings_UserId",
            //     table: "UserSettings",
            //     column: "UserId",
            //     unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDocuments_VehicleId",
                table: "VehicleDocuments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverRatings");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            // migrationBuilder.DropTable(
            //     name: "TwoFactorCodes");

            // migrationBuilder.DropTable(
            //     name: "UserProfiles");

            // migrationBuilder.DropTable(
            //     name: "UserSettings");

            migrationBuilder.DropTable(
                name: "VehicleDocuments");

            // migrationBuilder.DropTable(
            //     name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "DriverProfiles");
        }
    }
}
