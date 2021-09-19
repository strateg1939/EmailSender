using Microsoft.EntityFrameworkCore.Migrations;

namespace EmailSender.Migrations
{
    public partial class topicsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_connection_user_topic_AspNetUsers_AspNetUserID",
                table: "connection_user_topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_connection_user_topic",
                table: "connection_user_topic");

            migrationBuilder.DropIndex(
                name: "IX_connection_user_topic_TopicID",
                table: "connection_user_topic");

            migrationBuilder.DropColumn(
                name: "connection_user_topicID",
                table: "connection_user_topic");

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUserID",
                table: "connection_user_topic",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_connection_user_topic",
                table: "connection_user_topic",
                columns: new[] { "TopicID", "AspNetUserID" });

            migrationBuilder.AddForeignKey(
                name: "FK_connection_user_topic_AspNetUsers_AspNetUserID",
                table: "connection_user_topic",
                column: "AspNetUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_connection_user_topic_AspNetUsers_AspNetUserID",
                table: "connection_user_topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_connection_user_topic",
                table: "connection_user_topic");

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUserID",
                table: "connection_user_topic",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "connection_user_topicID",
                table: "connection_user_topic",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_connection_user_topic",
                table: "connection_user_topic",
                column: "connection_user_topicID");

            migrationBuilder.CreateIndex(
                name: "IX_connection_user_topic_TopicID",
                table: "connection_user_topic",
                column: "TopicID");

            migrationBuilder.AddForeignKey(
                name: "FK_connection_user_topic_AspNetUsers_AspNetUserID",
                table: "connection_user_topic",
                column: "AspNetUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
