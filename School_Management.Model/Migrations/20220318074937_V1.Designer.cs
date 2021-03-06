// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using School_Management.Model;

namespace School_Management.Model.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20220318074937_V1")]
    partial class V1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("School_Management.Model.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned")
                        .HasComment("Khóa chính");

                    b.Property<string>("Address")
                        .HasColumnName("address")
                        .HasColumnType("varchar(500)")
                        .HasComment("Địa chỉ")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(255)")
                        .HasComment("Tên phụ huynh")
                        .HasMaxLength(255);

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("varchar(50)")
                        .HasComment("Số điện thoại")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("parent");

                    b.HasComment("Phụ huynh học sinh");
                });

            modelBuilder.Entity("School_Management.Model.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned")
                        .HasComment("Khóa chính");

                    b.Property<string>("Class")
                        .HasColumnName("class")
                        .HasColumnType("text")
                        .HasComment("Lớp học");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnName("date_of_birt")
                        .HasColumnType("datetime")
                        .HasComment("Ngày sinh");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(255)")
                        .HasComment("Tên học sinh")
                        .HasMaxLength(255);

                    b.Property<int>("ParentId")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("id_UNIQUE");

                    b.HasIndex("ParentId");

                    b.ToTable("student");

                    b.HasComment("Học sinh");
                });

            modelBuilder.Entity("School_Management.Model.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int unsigned")
                        .HasComment("Khóa chính");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnName("date_of_birt")
                        .HasColumnType("datetime")
                        .HasComment("Ngày sinh");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(100)")
                        .HasComment("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(255)")
                        .HasComment("Tên giáo viên")
                        .HasMaxLength(255);

                    b.Property<string>("Phone")
                        .HasColumnName("phone")
                        .HasColumnType("varchar(50)")
                        .HasComment("Số điện thoại")
                        .HasMaxLength(50);

                    b.Property<string>("Subject")
                        .HasColumnName("subject")
                        .HasColumnType("varchar(100)")
                        .HasComment("Bộ môn")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("teacher");

                    b.HasComment("Giáo viên");
                });

            modelBuilder.Entity("School_Management.Model.Student", b =>
                {
                    b.HasOne("School_Management.Model.Parent", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("FK_Students_Parent")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
