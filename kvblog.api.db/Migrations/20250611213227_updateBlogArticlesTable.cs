using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kvblog.Api.Db.Migrations
{
    /// <inheritdoc />
    public partial class updateBlogArticlesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedImageUrl",
                table: "BlogArticles");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BlogArticles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5000)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "BlogArticles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePosted",
                table: "BlogArticles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "BlogArticles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "BlogArticles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("30c220be-28c7-49a2-945f-d656f7631b1d"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2020, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-10", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)10" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("5b0be4d7-90aa-4588-a13e-bf71f7b5ad5f"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-1", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)1" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("79df870d-3016-4cc9-8fb7-182315cf9743"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-3", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)3" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("90ff393f-d868-4df9-9830-47fea72812de"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-7", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)7" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("9a55afdf-1fb3-4518-9843-fa31a947b5f3"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2022, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-8", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)8" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("b9592224-ae00-45eb-af54-e3631c296973"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2022, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 9, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-5", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)5" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("bfe0c948-edfe-4216-a274-2a3baf6de542"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2024, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-2", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)2" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("cfc5d387-9c60-485b-ad92-e1480ac81a9f"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2022, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 8, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-6", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)6" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("d5431b7a-01dc-4d1a-9684-112eeb4a722d"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-4", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)4" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("f9796233-836c-427d-9587-e826a1be080b"),
                columns: new[] { "DatePosted", "DateUpdated", "Slug", "Title" },
                values: new object[] { new DateTime(2021, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2021, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), "whats-new-in-system-text-json-in-dotnet-9-lorem-ipsum-post-9", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "BlogArticles");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BlogArticles",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "BlogArticles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePosted",
                table: "BlogArticles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "BlogArticles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImageUrl",
                table: "BlogArticles",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("30c220be-28c7-49a2-945f-d656f7631b1d"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2632), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2632), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("5b0be4d7-90aa-4588-a13e-bf71f7b5ad5f"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2612), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2614), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("79df870d-3016-4cc9-8fb7-182315cf9743"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2618), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2619), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("90ff393f-d868-4df9-9830-47fea72812de"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2626), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2626), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("9a55afdf-1fb3-4518-9843-fa31a947b5f3"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2628), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2628), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("b9592224-ae00-45eb-af54-e3631c296973"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2622), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2623), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("bfe0c948-edfe-4216-a274-2a3baf6de542"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2616), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2616), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("cfc5d387-9c60-485b-ad92-e1480ac81a9f"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2624), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2625), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("d5431b7a-01dc-4d1a-9684-112eeb4a722d"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2620), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2621), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });

            migrationBuilder.UpdateData(
                table: "BlogArticles",
                keyColumn: "Id",
                keyValue: new Guid("f9796233-836c-427d-9587-e826a1be080b"),
                columns: new[] { "DatePosted", "DateUpdated", "FeaturedImageUrl", "Title" },
                values: new object[] { new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2630), new DateTime(2025, 5, 24, 9, 33, 5, 504, DateTimeKind.Utc).AddTicks(2630), "defaultArticle.jpg", "What’s new in System.Text.Json in .NET 9 (lorem ipsum post)" });
        }
    }
}
