﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestApplication;

namespace TestApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210506090947_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("TestApplication.Domain.StudentModels.Student", b =>
                {
                    b.Property<string>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Grade")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentName")
                        .HasColumnType("TEXT");

                    b.HasKey("StudentId");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("TestApplication.Domain.StudentModels.Subject", b =>
                {
                    b.Property<string>("SubjectId")
                        .HasColumnType("TEXT");

                    b.Property<int>("English")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Grade")
                        .HasColumnType("TEXT");

                    b.Property<int>("Maths")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Nepali")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Percentage")
                        .HasColumnType("REAL");

                    b.Property<int>("Science")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Social")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StudentId")
                        .HasColumnType("TEXT");

                    b.Property<float>("TotalMarks")
                        .HasColumnType("REAL");

                    b.HasKey("SubjectId");

                    b.ToTable("Subjects");
                });
#pragma warning restore 612, 618
        }
    }
}