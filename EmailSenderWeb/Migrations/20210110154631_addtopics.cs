using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailSender.Migrations
{
    public partial class addtopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Topic_name" },
                values: new object[] { 1, "news" });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Topic_name" },
                values: new object[] { 2, "finance" });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Topic_name" },
                values: new object[] { 3, "sport" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "TopicId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "TopicId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "TopicId",
                keyValue: 3);
        }
    }
}
