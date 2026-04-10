using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMultimedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AccessType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DurationSeconds = table.Column<int>(type: "int", nullable: true),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    AuthorMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaContents_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaContents_Members_AuthorMemberId",
                        column: x => x.AuthorMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaPromotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaPromotions_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaPromotions_MediaContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "MediaContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PaymentReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivatedByMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaPurchases_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaPurchases_MediaContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "MediaContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaPurchases_Members_ActivatedByMemberId",
                        column: x => x.ActivatedByMemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaPurchases_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "69ebc1be-c8b4-45de-96dd-7f628521a761");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "c39df13e-ba1c-4322-a2f0-3341e54c7818");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "97e9b563-a764-4c20-a2a0-72faadb54500");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "2ab67727-2002-483c-bf8c-62685e1907f3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "b89e16ca-cc30-4d3b-a712-c993d418d93a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "d79c3393-9eea-427d-91ec-dec0ee9ab345");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "e003f48a-3bee-44ce-b9aa-a99178591267");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "6cb3ee02-3a99-46cd-9aaf-e6232aa8dcfb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "10161875-a85d-4b81-99c0-46158da5e80b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "0c5eb6be-b52d-4f34-ae50-94482e1f0720");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "3627e355-723f-4ee1-bd66-a8f5a2a4e2a3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "576dd894-0791-474c-81a6-3957157d7703");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "f2c36fee-f323-48c8-abbe-9a462995088c");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_AuthorMemberId",
                table: "MediaContents",
                column: "AuthorMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_ChurchId",
                table: "MediaContents",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_ContentType",
                table: "MediaContents",
                column: "ContentType");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_Status",
                table: "MediaContents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPromotions_ChurchId",
                table: "MediaPromotions",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPromotions_Code",
                table: "MediaPromotions",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPromotions_ContentId",
                table: "MediaPromotions",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPromotions_IsActive",
                table: "MediaPromotions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPurchases_ActivatedByMemberId",
                table: "MediaPurchases",
                column: "ActivatedByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPurchases_ChurchId",
                table: "MediaPurchases",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPurchases_ContentId",
                table: "MediaPurchases",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPurchases_MemberId",
                table: "MediaPurchases",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaPurchases_Status",
                table: "MediaPurchases",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaPromotions");

            migrationBuilder.DropTable(
                name: "MediaPurchases");

            migrationBuilder.DropTable(
                name: "MediaContents");

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
        }
    }
}
