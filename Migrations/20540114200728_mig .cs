using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailSender.Migrations
{
    public partial class mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DeleteData(
                 table: "Articles",
                 keyColumn: "ArticleId",
                 keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "Articles",
               columns: new[] { "ArticleId", "TopicID", "date", "Article_text" },
               values: new object[] { 1, 1, 12 - 12 - 2021, "some news text" });


        }
    }
}
