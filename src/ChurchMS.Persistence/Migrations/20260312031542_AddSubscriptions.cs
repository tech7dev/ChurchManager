using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChurchMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsCredits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    TotalPurchased = table.Column<int>(type: "int", nullable: false),
                    TotalConsumed = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsCredits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsCredits_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BillingCycle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AutoRenew = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrialEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextBillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExternalSubscriptionId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmsCreditTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SmsCreditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    BalanceBefore = table.Column<int>(type: "int", nullable: false),
                    BalanceAfter = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsCreditTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsCreditTransactions_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmsCreditTransactions_SmsCredits_SmsCreditId",
                        column: x => x.SmsCreditId,
                        principalTable: "SmsCredits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ChurchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Churches_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Churches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "3bac9e28-fb58-4c40-92aa-63d6f2209019");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "36cdc518-e1b7-4587-9243-a07ac836d917");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "a2ab87c2-4485-4005-9af8-d456743c2c23");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "b34237f8-403f-47ba-acc2-1e77fe3db69f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "691cccf2-5acf-4856-8345-e69125986f51");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "a35ca870-3212-4639-a49e-dc0c08f2729c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "5f648643-049c-4f9a-997c-7f8a1c5c6067");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "108af76a-873a-4b29-9cfb-11392ea964fe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "b2cd255b-5884-4c15-9d6e-4befe079173a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "987defc7-75fd-496a-8c3e-47a518fc5d2c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "b362876a-b3de-4b78-a3ca-cd75a7f3f70d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "fe13bcfe-b876-45ef-a52d-66647de657e4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "2c8b9c56-a1b0-42b7-b7a1-81d2e0b0863c");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ChurchId",
                table: "Invoices",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DueDate",
                table: "Invoices",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Status",
                table: "Invoices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SubscriptionId",
                table: "Invoices",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsCredits_ChurchId",
                table: "SmsCredits",
                column: "ChurchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsCreditTransactions_ChurchId",
                table: "SmsCreditTransactions",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsCreditTransactions_CreatedAt",
                table: "SmsCreditTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SmsCreditTransactions_SmsCreditId",
                table: "SmsCreditTransactions",
                column: "SmsCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ChurchId",
                table: "Subscriptions",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Status",
                table: "Subscriptions",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "SmsCreditTransactions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SmsCredits");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("12c86dfe-37d2-a015-0200-3b37e5ddd6cf"),
                column: "ConcurrencyStamp",
                value: "be12ff9d-f47c-4d5d-ad03-ee7e449ded69");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1449cac4-dbd5-eca9-306f-ae0631614598"),
                column: "ConcurrencyStamp",
                value: "fe26902c-b79b-4a37-934c-14c7bdac47e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("181fb183-29cf-98ab-9bf5-0e6586afbb98"),
                column: "ConcurrencyStamp",
                value: "3bdffc37-3e3b-47f0-9941-df6d6bc8b461");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ed18ce4-0485-3781-7e5a-9fa66c22c168"),
                column: "ConcurrencyStamp",
                value: "440048c7-f534-4232-a9e9-368b3f80d2ab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2ee5ee5e-e5c1-9880-9be9-44ee7cae917d"),
                column: "ConcurrencyStamp",
                value: "34e41251-c3c9-4832-813b-06c93b5c3680");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3565871d-a91b-1b69-044a-2a32f9aac7b5"),
                column: "ConcurrencyStamp",
                value: "44b9a439-f630-48c0-aec9-956dd91a43d2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("613a4df9-4bfc-2d32-e134-ab222d849e50"),
                column: "ConcurrencyStamp",
                value: "dcea2dde-7274-41c3-8435-a9e860723a2b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6a999513-6810-6dd2-cb84-86df1e1db89b"),
                column: "ConcurrencyStamp",
                value: "322a31be-2f1c-432a-9263-404306bc2386");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf621d7-ea3b-c90e-eb5a-029aac85ef24"),
                column: "ConcurrencyStamp",
                value: "4029ac88-03d3-4339-9336-20dc5d18a36e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("73a7f390-b4fe-f40a-0179-9ef804de8ec0"),
                column: "ConcurrencyStamp",
                value: "74b1ff2a-e4a5-4b44-8ff0-60808ca02f9f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b78f967c-501f-35e3-442b-35062a35620a"),
                column: "ConcurrencyStamp",
                value: "9f667f9a-2fe4-45b7-be83-4649e1b12805");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d03e059b-fea6-ec2d-e316-6a9526406a17"),
                column: "ConcurrencyStamp",
                value: "0a749a0b-35f9-4fe2-b660-cfc57f5e78e7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f9893397-d7a8-e694-78d9-1c5c9e60d271"),
                column: "ConcurrencyStamp",
                value: "3694fda2-3c28-4bea-8947-0932665cb059");
        }
    }
}
