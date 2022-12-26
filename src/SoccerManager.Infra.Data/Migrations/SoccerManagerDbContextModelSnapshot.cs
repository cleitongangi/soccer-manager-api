﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoccerManager.Infra.Data.Context;

#nullable disable

namespace SoccerManager.Infra.Data.Migrations
{
    [DbContext(typeof(SoccerManagerDbContext))]
    partial class SoccerManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SoccerManager.Domain.Entities.PlayerEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("Age");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Country");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LastName");

                    b.Property<decimal>("MarketValue")
                        .HasColumnType("money")
                        .HasColumnName("MarketValue");

                    b.Property<short>("PositionId")
                        .HasColumnType("smallint")
                        .HasColumnName("PositionId");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id")
                        .HasName("PK_Players");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.HasIndex("PositionId");

                    b.ToTable("Players", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.PlayerPositionEntity", b =>
                {
                    b.Property<short>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("Id");

                    b.Property<string>("PositionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("PositionName");

                    b.HasKey("Id")
                        .HasName("PK_PlayerPositions");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("PlayerPositions", "dbo");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            PositionName = "Goalkeeper"
                        },
                        new
                        {
                            Id = (short)2,
                            PositionName = "Defender"
                        },
                        new
                        {
                            Id = (short)3,
                            PositionName = "Midfielder"
                        },
                        new
                        {
                            Id = (short)4,
                            PositionName = "Attacker"
                        });
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TeamEntity", b =>
                {
                    b.Property<long>("TeamId")
                        .HasColumnType("bigint")
                        .HasColumnName("TeamId");

                    b.Property<decimal>("Budget")
                        .HasColumnType("money")
                        .HasColumnName("Budget");

                    b.Property<string>("TeamCountry")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("TeamCountry");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("TeamName");

                    b.HasKey("TeamId")
                        .HasName("PK_Teams");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("TeamId"));

                    b.ToTable("Teams", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TeamPlayerEntity", b =>
                {
                    b.Property<long>("TeamId")
                        .HasColumnType("bigint")
                        .HasColumnName("TeamId");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("PlayerId");

                    b.Property<int>("Sequence")
                        .HasColumnType("int")
                        .HasColumnName("Sequence");

                    b.Property<bool>("Active")
                        .HasColumnType("bit")
                        .HasColumnName("Active");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("RemovedAt");

                    b.HasKey("TeamId", "PlayerId", "Sequence")
                        .HasName("PK_TeamPlayers");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("TeamId", "PlayerId", "Sequence"));

                    b.HasIndex("PlayerId");

                    b.ToTable("TeamPlayers", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TransferListEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint")
                        .HasColumnName("PlayerId");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasColumnName("Price");

                    b.Property<long>("SourceTeamId")
                        .HasColumnType("bigint")
                        .HasColumnName("SourceTeamId");

                    b.Property<short>("StatusId")
                        .HasColumnType("smallint")
                        .HasColumnName("StatusId");

                    b.Property<long?>("TargetTeamId")
                        .HasColumnType("bigint")
                        .HasColumnName("TargetTeamId");

                    b.Property<DateTime?>("TransferedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("TransferedAt");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("UpdatedAt");

                    b.HasKey("Id")
                        .HasName("PK_TransferList");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.HasIndex("PlayerId");

                    b.HasIndex("SourceTeamId");

                    b.HasIndex("StatusId");

                    b.HasIndex("TargetTeamId");

                    b.ToTable("TransferList", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TransferListStatusEntity", b =>
                {
                    b.Property<short>("Id")
                        .HasColumnType("smallint")
                        .HasColumnName("Id");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("varchar(11)")
                        .HasColumnName("StatusName");

                    b.HasKey("Id")
                        .HasName("PK_TransferListStatus");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("TransferListStatus", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("Password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Username");

                    b.HasKey("Id")
                        .HasName("PK_Users");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.PlayerEntity", b =>
                {
                    b.HasOne("SoccerManager.Domain.Entities.PlayerPositionEntity", "PlayerPosition")
                        .WithMany("Players")
                        .HasForeignKey("PositionId")
                        .IsRequired()
                        .HasConstraintName("FK_Players_PlayerPositions");

                    b.Navigation("PlayerPosition");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TeamEntity", b =>
                {
                    b.HasOne("SoccerManager.Domain.Entities.UserEntity", "User")
                        .WithOne("Team")
                        .HasForeignKey("SoccerManager.Domain.Entities.TeamEntity", "TeamId")
                        .IsRequired()
                        .HasConstraintName("FK_Teams_Users");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TeamPlayerEntity", b =>
                {
                    b.HasOne("SoccerManager.Domain.Entities.PlayerEntity", "Player")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("FK_TeamPlayers_Players");

                    b.HasOne("SoccerManager.Domain.Entities.TeamEntity", "Team")
                        .WithMany("TeamPlayers")
                        .HasForeignKey("TeamId")
                        .IsRequired()
                        .HasConstraintName("FK_TeamPlayers_Teams");

                    b.Navigation("Player");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TransferListEntity", b =>
                {
                    b.HasOne("SoccerManager.Domain.Entities.PlayerEntity", "Player")
                        .WithMany("TransferLists")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("FK_TransferList_Players");

                    b.HasOne("SoccerManager.Domain.Entities.TeamEntity", "SourceTeam")
                        .WithMany("TransferLists_SourceTeams")
                        .HasForeignKey("SourceTeamId")
                        .IsRequired()
                        .HasConstraintName("FK_TransferList_SourceTeam");

                    b.HasOne("SoccerManager.Domain.Entities.TransferListStatusEntity", "TransferListStatus")
                        .WithMany("TransferLists")
                        .HasForeignKey("StatusId")
                        .IsRequired()
                        .HasConstraintName("FK_TransferList_TransferListStatus");

                    b.HasOne("SoccerManager.Domain.Entities.TeamEntity", "TargetTeam")
                        .WithMany("TransferLists_TargetTeams")
                        .HasForeignKey("TargetTeamId")
                        .HasConstraintName("FK_TransferList_TargetTeam");

                    b.Navigation("Player");

                    b.Navigation("SourceTeam");

                    b.Navigation("TargetTeam");

                    b.Navigation("TransferListStatus");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.PlayerEntity", b =>
                {
                    b.Navigation("TeamPlayers");

                    b.Navigation("TransferLists");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.PlayerPositionEntity", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TeamEntity", b =>
                {
                    b.Navigation("TeamPlayers");

                    b.Navigation("TransferLists_SourceTeams");

                    b.Navigation("TransferLists_TargetTeams");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.TransferListStatusEntity", b =>
                {
                    b.Navigation("TransferLists");
                });

            modelBuilder.Entity("SoccerManager.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("Team");
                });
#pragma warning restore 612, 618
        }
    }
}