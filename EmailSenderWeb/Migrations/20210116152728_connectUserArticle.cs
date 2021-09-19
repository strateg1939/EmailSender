using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailSender.Migrations
{
    public partial class connectUserArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "connection_user_article",
                columns: table => new
                {
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection_user_article", x => new { x.ArticleId, x.AspNetUserId });
                    table.ForeignKey(
                        name: "FK_connection_user_article_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_connection_user_article_AspNetUsers_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_connection_user_article_AspNetUserId",
                table: "connection_user_article",
                column: "AspNetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connection_user_article");

            
        }
    }
}
