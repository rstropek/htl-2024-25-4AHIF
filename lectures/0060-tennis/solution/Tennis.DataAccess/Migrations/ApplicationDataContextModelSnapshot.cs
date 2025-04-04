﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tennis.DataAccess;

#nullable disable

namespace Tennis.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    partial class ApplicationDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Tennis.DataAccess.CurrentGameScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GameScoreJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("CurrentGameScores");
                });

            modelBuilder.Entity("Tennis.DataAccess.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BestOfSets")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("EndDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Player1Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Player2Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("TournamentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Winner")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Tennis.DataAccess.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ace")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("TEXT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Net")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Out")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ScoringPlayer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServeType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServingPlayer")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("Tennis.DataAccess.CurrentGameScore", b =>
                {
                    b.HasOne("Tennis.DataAccess.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Tennis.DataAccess.Point", b =>
                {
                    b.HasOne("Tennis.DataAccess.Game", "Game")
                        .WithMany("Points")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Tennis.DataAccess.Game", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
