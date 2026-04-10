using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSecretariat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssuedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IssuedByMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Certificates_Members_IssuedByMemberId",
                        column: x => x.IssuedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UploadedByMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Members_UploadedByMemberId",
                        column: x => x.UploadedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaptismRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaptismDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OfficiantMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CertificateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaptismRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaptismRecords_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaptismRecords_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaptismRecords_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaptismRecords_Members_OfficiantMemberId",
                        column: x => x.OfficiantMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarriageRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Spouse1MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Spouse2MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Spouse2Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Spouse2Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MarriageDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OfficiantMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CertificateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarriageRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarriageRecords_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarriageRecords_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MarriageRecords_Members_OfficiantMemberId",
                        column: x => x.OfficiantMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarriageRecords_Members_Spouse1MemberId",
                        column: x => x.Spouse1MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarriageRecords_Members_Spouse2MemberId",
                        column: x => x.Spouse2MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "268b184a-976b-4ef7-94ad-1e1b8cae18db");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "807f8e54-aab6-4f50-8067-f505c4c98b0e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "d2eb613f-7e1e-471b-90f3-1b30aa60f928");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "67e96ddc-d60b-4f95-b854-8b24f0fdc598");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "62a0a30b-5523-42ce-b9c8-e4e98df5ceb6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "fbba5702-5af2-4633-9cc5-40170ae6b08c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "6300e6ae-6c1d-45d9-8efe-a1d17d803638");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "d9db5045-ce71-42a9-ab08-d494ba33baa0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "8435bff7-a8a9-48f7-9078-82697911182c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "31b04689-d46d-42f1-83f0-c7890126aa36");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "4f85f628-a17f-439f-a4c4-55b4167957a4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "636dfa7f-31e4-4d8f-afc8-bc2b57db6678");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "b17f6de7-83ca-439c-b85b-313ab3a48f71");

            migrationBuilder.CreateIndex(
                name: "IX_BaptismRecords_CertificateId",
                table: "BaptismRecords",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_BaptismRecords_ChurchId",
                table: "BaptismRecords",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_BaptismRecords_MemberId",
                table: "BaptismRecords",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_BaptismRecords_OfficiantMemberId",
                table: "BaptismRecords",
                column: "OfficiantMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ChurchId",
                table: "Certificates",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ChurchId_CertificateNumber",
                table: "Certificates",
                columns: new[] { "ChurchId", "CertificateNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_IssuedByMemberId",
                table: "Certificates",
                column: "IssuedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MemberId",
                table: "Certificates",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ChurchId",
                table: "Documents",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MemberId",
                table: "Documents",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Type",
                table: "Documents",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UploadedByMemberId",
                table: "Documents",
                column: "UploadedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageRecords_CertificateId",
                table: "MarriageRecords",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageRecords_ChurchId",
                table: "MarriageRecords",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageRecords_OfficiantMemberId",
                table: "MarriageRecords",
                column: "OfficiantMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageRecords_Spouse1MemberId",
                table: "MarriageRecords",
                column: "Spouse1MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageRecords_Spouse2MemberId",
                table: "MarriageRecords",
                column: "Spouse2MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaptismRecords");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "MarriageRecords");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "33096d6b-b2c0-47c4-93d9-ec8587fef2ad");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "8facb6ef-f4e2-434a-9587-85d8845e3835");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "bd74fb04-c922-44e3-b98f-2777c0b0e993");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "9807abe6-dd77-41d4-9896-b2f2f57cd676");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "0bf54666-9607-4019-bd59-202b02e01826");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "6f2db23b-2211-4ebd-b2ba-49d34c6bed21");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "a6e210a7-4402-485e-ab7b-fbb5bc773cc3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "797d4817-26d8-4ece-a40f-e066fe13b5d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "7c4714cf-8f19-442e-9be5-4cbac64f0b9d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "01c34af7-2db8-4388-a426-9c4329a18378");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "de3bc756-9567-403e-9f8b-eb008a68adb7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "1c13eafc-b586-40cd-8822-5c23631f9f43");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "e3393e5d-490d-4c2f-bcbb-adae9d48c2e2");
        }
    }
}
