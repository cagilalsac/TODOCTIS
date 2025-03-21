﻿// <auto-generated />
using System;
using APP.TODO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APP.TODO.Migrations
{
    [DbContext(typeof(TodoDb))]
    [Migration("20250305161340_v2")]
    partial class v2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.13");

            modelBuilder.Entity("APP.TODO.Domain.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CompletePercentage")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Todo");
                });

            modelBuilder.Entity("APP.TODO.Domain.TodoTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TodoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TopicId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TodoId");

                    b.HasIndex("TopicId");

                    b.ToTable("TodoTopic");
                });

            modelBuilder.Entity("APP.TODO.Domain.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("APP.TODO.Domain.TodoTopic", b =>
                {
                    b.HasOne("APP.TODO.Domain.Todo", "Todo")
                        .WithMany("TodoTopics")
                        .HasForeignKey("TodoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APP.TODO.Domain.Topic", "Topic")
                        .WithMany("TodoTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Todo");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("APP.TODO.Domain.Todo", b =>
                {
                    b.Navigation("TodoTopics");
                });

            modelBuilder.Entity("APP.TODO.Domain.Topic", b =>
                {
                    b.Navigation("TodoTopics");
                });
#pragma warning restore 612, 618
        }
    }
}
