using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace School_Management.Model.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parent",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false, comment: "Khóa chính")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 255, nullable: false, comment: "Tên phụ huynh"),
                    phone = table.Column<string>(maxLength: 50, nullable: true, comment: "Số điện thoại"),
                    address = table.Column<string>(maxLength: 500, nullable: true, comment: "Địa chỉ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parent", x => x.id);
                },
                comment: "Phụ huynh học sinh");

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false, comment: "Khóa chính")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 255, nullable: false, comment: "Tên giáo viên"),
                    subject = table.Column<string>(maxLength: 100, nullable: true, comment: "Bộ môn"),
                    date_of_birt = table.Column<DateTime>(nullable: false, comment: "Ngày sinh"),
                    phone = table.Column<string>(maxLength: 50, nullable: true, comment: "Số điện thoại"),
                    email = table.Column<string>(maxLength: 100, nullable: true, comment: "Email")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.id);
                },
                comment: "Giáo viên");

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "int unsigned", nullable: false, comment: "Khóa chính")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 255, nullable: false, comment: "Tên học sinh"),
                    date_of_birt = table.Column<DateTime>(nullable: false, comment: "Ngày sinh"),
                    @class = table.Column<string>(name: "class", nullable: true, comment: "Lớp học"),
                    ParentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_Students_Parent",
                        column: x => x.ParentId,
                        principalTable: "parent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Học sinh");

            migrationBuilder.CreateIndex(
                name: "id_UNIQUE",
                table: "student",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_ParentId",
                table: "student",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "teacher");

            migrationBuilder.DropTable(
                name: "parent");
        }
    }
}
