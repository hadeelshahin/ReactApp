﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230107200228_DbMigration")]
    partial class DbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ModelsShared.Models.Answers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAnswerTrue")
                        .HasColumnType("bit");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("ModelsShared.Models.Classes", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("ModelsShared.Models.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("QuizListId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuizListId");

                    b.ToTable("Quiz");
                });

            modelBuilder.Entity("ModelsShared.Models.QuizList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClassesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FromTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsMarker")
                        .HasColumnType("bit");

                    b.Property<int>("QuizTimeType")
                        .HasColumnType("int");

                    b.Property<int>("QuizType")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ToTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClassesId");

                    b.ToTable("QuizList");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StudentQustionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("StudentQustionId");

                    b.ToTable("StudentAnswer");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentAnswersQustions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("QuizListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QuizListId");

                    b.HasIndex("UserId");

                    b.ToTable("StudentAnswersQustions");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentQustion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("QustionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StudentAnswersQustionsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("QustionId");

                    b.HasIndex("StudentAnswersQustionsId");

                    b.ToTable("StudentQustion");
                });

            modelBuilder.Entity("ModelsShared.Models.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniversityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ModelsShared.Models.UsersClass", b =>
                {
                    b.Property<Guid>("ClassId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ClassId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersClass");
                });

            modelBuilder.Entity("ModelsShared.Models.Answers", b =>
                {
                    b.HasOne("ModelsShared.Models.Quiz", "Quiz")
                        .WithMany("Answers")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("ModelsShared.Models.Classes", b =>
                {
                    b.HasOne("ModelsShared.Models.Users", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("ModelsShared.Models.Quiz", b =>
                {
                    b.HasOne("ModelsShared.Models.QuizList", "QuizList")
                        .WithMany("Quizzes")
                        .HasForeignKey("QuizListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizList");
                });

            modelBuilder.Entity("ModelsShared.Models.QuizList", b =>
                {
                    b.HasOne("ModelsShared.Models.Classes", "Classes")
                        .WithMany("QuizList")
                        .HasForeignKey("ClassesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classes");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentAnswer", b =>
                {
                    b.HasOne("ModelsShared.Models.Answers", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsShared.Models.StudentQustion", "StudentQustion")
                        .WithMany("StudentAnswer")
                        .HasForeignKey("StudentQustionId");

                    b.Navigation("Answer");

                    b.Navigation("StudentQustion");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentAnswersQustions", b =>
                {
                    b.HasOne("ModelsShared.Models.QuizList", "QuizList")
                        .WithMany()
                        .HasForeignKey("QuizListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsShared.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentQustion", b =>
                {
                    b.HasOne("ModelsShared.Models.Quiz", "Qustion")
                        .WithMany()
                        .HasForeignKey("QustionId");

                    b.HasOne("ModelsShared.Models.StudentAnswersQustions", "StudentAnswersQustions")
                        .WithMany("StudentQustion")
                        .HasForeignKey("StudentAnswersQustionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Qustion");

                    b.Navigation("StudentAnswersQustions");
                });

            modelBuilder.Entity("ModelsShared.Models.UsersClass", b =>
                {
                    b.HasOne("ModelsShared.Models.Classes", "Class")
                        .WithMany("Students")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelsShared.Models.Users", "User")
                        .WithMany("Classes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ModelsShared.Models.Classes", b =>
                {
                    b.Navigation("QuizList");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("ModelsShared.Models.Quiz", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("ModelsShared.Models.QuizList", b =>
                {
                    b.Navigation("Quizzes");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentAnswersQustions", b =>
                {
                    b.Navigation("StudentQustion");
                });

            modelBuilder.Entity("ModelsShared.Models.StudentQustion", b =>
                {
                    b.Navigation("StudentAnswer");
                });

            modelBuilder.Entity("ModelsShared.Models.Users", b =>
                {
                    b.Navigation("Classes");
                });
#pragma warning restore 612, 618
        }
    }
}
