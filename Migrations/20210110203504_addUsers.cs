using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailSender.Migrations
{
    public partial class addUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "connection_user_topic",
                columns: table => new
                {
                    connection_user_topicID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AspNetUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TopicID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection_user_topic", x => x.connection_user_topicID);
                    table.ForeignKey(
                        name: "FK_connection_user_topic_AspNetUsers_AspNetUserID",
                        column: x => x.AspNetUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_connection_user_topic_Topics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_connection_user_topic_AspNetUserID",
                table: "connection_user_topic",
                column: "AspNetUserID");

            migrationBuilder.CreateIndex(
                name: "IX_connection_user_topic_TopicID",
                table: "connection_user_topic",
                column: "TopicID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connection_user_topic");
        }
    }
}
