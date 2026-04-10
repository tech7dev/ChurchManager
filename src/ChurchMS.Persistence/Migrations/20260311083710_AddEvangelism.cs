using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEvangelism : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvangelismCampaigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    GoalContacts = table.Column<int>(type: "int", nullable: true),
                    LeaderMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_EvangelismCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvangelismCampaigns_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismCampaigns_Members_LeaderMemberId",
                        column: x => x.LeaderMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvangelismTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LeaderMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_EvangelismTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvangelismTeams_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismTeams_EvangelismCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "EvangelismCampaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismTeams_Members_LeaderMemberId",
                        column: x => x.LeaderMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvangelismContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AssignedToMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ConvertedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConvertedMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvangelismContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvangelismContacts_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismContacts_EvangelismCampaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "EvangelismCampaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismContacts_EvangelismTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "EvangelismTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvangelismContacts_Members_AssignedToMemberId",
                        column: x => x.AssignedToMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvangelismContacts_Members_ConvertedMemberId",
                        column: x => x.ConvertedMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvangelismTeamMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoinedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvangelismTeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvangelismTeamMembers_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismTeamMembers_EvangelismTeams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "EvangelismTeams",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismTeamMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvangelismFollowUps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FollowUpDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ConductedByMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvangelismFollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvangelismFollowUps_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismFollowUps_EvangelismContacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "EvangelismContacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EvangelismFollowUps_Members_ConductedByMemberId",
                        column: x => x.ConductedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "bc64b25c-00fc-4111-b8a4-c372b032fdb5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "f947355f-1c57-45e8-a287-ac6f8d6215c5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "b1c075f9-ee48-43b2-85f8-d0e90301372c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "e02d9bd5-2ab5-4674-8bbc-e60d26dad410");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "c3bd41e5-d817-408a-bdce-dfdf52b3d765");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "b392df14-2336-450c-b851-62258b554bab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "3fa1a232-e010-4d96-a06a-fb0428be8277");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "15931e9a-8950-4dfb-b16b-be5ddce3f99b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "baca33c1-1076-4991-af79-53da12cc66b1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "fee468f2-2401-4f92-871b-61bd432f283f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "87ba8011-9956-43e9-9811-b8b0f11b4905");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "c2c462dd-db30-41b6-a137-362612f4c757");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "4fc329d8-b24e-417e-9934-57a948988813");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismCampaigns_ChurchId",
                table: "EvangelismCampaigns",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismCampaigns_LeaderMemberId",
                table: "EvangelismCampaigns",
                column: "LeaderMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismCampaigns_Status",
                table: "EvangelismCampaigns",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_AssignedToMemberId",
                table: "EvangelismContacts",
                column: "AssignedToMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_CampaignId",
                table: "EvangelismContacts",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_ChurchId",
                table: "EvangelismContacts",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_ConvertedMemberId",
                table: "EvangelismContacts",
                column: "ConvertedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_Status",
                table: "EvangelismContacts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismContacts_TeamId",
                table: "EvangelismContacts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismFollowUps_ChurchId",
                table: "EvangelismFollowUps",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismFollowUps_ConductedByMemberId",
                table: "EvangelismFollowUps",
                column: "ConductedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismFollowUps_ContactId",
                table: "EvangelismFollowUps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeamMembers_ChurchId",
                table: "EvangelismTeamMembers",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeamMembers_MemberId",
                table: "EvangelismTeamMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeamMembers_TeamId_MemberId",
                table: "EvangelismTeamMembers",
                columns: new[] { "TeamId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeams_CampaignId",
                table: "EvangelismTeams",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeams_ChurchId",
                table: "EvangelismTeams",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_EvangelismTeams_LeaderMemberId",
                table: "EvangelismTeams",
                column: "LeaderMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvangelismFollowUps");

            migrationBuilder.DropTable(
                name: "EvangelismTeamMembers");

            migrationBuilder.DropTable(
                name: "EvangelismContacts");

            migrationBuilder.DropTable(
                name: "EvangelismTeams");

            migrationBuilder.DropTable(
                name: "EvangelismCampaigns");

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
        }
    }
}
