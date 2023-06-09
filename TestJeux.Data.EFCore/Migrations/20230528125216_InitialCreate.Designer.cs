﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestJeux.Core.Context;

#nullable disable

namespace TestJeux.Data.EFCore.Migrations
{
    [DbContext(typeof(GameContext))]
    [Migration("20230528125216_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Decoration", b =>
                {
                    b.Property<int>("DecorationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DecorationId"));

                    b.Property<int>("LevelId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.HasKey("DecorationId");

                    b.HasIndex("LevelId");

                    b.ToTable("Decorations");
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemId"));

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("DefaultState")
                        .HasColumnType("integer");

                    b.Property<int>("LevelId")
                        .HasColumnType("integer");

                    b.Property<int>("Orientation")
                        .HasColumnType("integer");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.HasKey("ItemId");

                    b.HasIndex("LevelId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Level", b =>
                {
                    b.Property<int>("LevelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LevelId"));

                    b.Property<int>("Music")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Shader")
                        .HasColumnType("integer");

                    b.HasKey("LevelId");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Tile", b =>
                {
                    b.Property<int>("TileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TileId"));

                    b.Property<int>("Angle")
                        .HasColumnType("integer");

                    b.Property<int>("LevelId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("X1")
                        .HasColumnType("integer");

                    b.Property<int>("X2")
                        .HasColumnType("integer");

                    b.Property<int>("Y1")
                        .HasColumnType("integer");

                    b.Property<int>("Y2")
                        .HasColumnType("integer");

                    b.HasKey("TileId");

                    b.HasIndex("LevelId");

                    b.ToTable("Tiles");
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Zone", b =>
                {
                    b.Property<int>("ZoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ZoneId"));

                    b.Property<int>("GroundType")
                        .HasColumnType("integer");

                    b.Property<int>("LevelId")
                        .HasColumnType("integer");

                    b.Property<int>("X1")
                        .HasColumnType("integer");

                    b.Property<int>("X2")
                        .HasColumnType("integer");

                    b.Property<int>("Y1")
                        .HasColumnType("integer");

                    b.Property<int>("Y2")
                        .HasColumnType("integer");

                    b.HasKey("ZoneId");

                    b.HasIndex("LevelId");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Decoration", b =>
                {
                    b.HasOne("TestJeux.Data.EFCore.DbItems.Level", null)
                        .WithMany("Decorations")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Item", b =>
                {
                    b.HasOne("TestJeux.Data.EFCore.DbItems.Level", null)
                        .WithMany("Items")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Tile", b =>
                {
                    b.HasOne("TestJeux.Data.EFCore.DbItems.Level", null)
                        .WithMany("Tiles")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Zone", b =>
                {
                    b.HasOne("TestJeux.Data.EFCore.DbItems.Level", null)
                        .WithMany("Zones")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestJeux.Data.EFCore.DbItems.Level", b =>
                {
                    b.Navigation("Decorations");

                    b.Navigation("Items");

                    b.Navigation("Tiles");

                    b.Navigation("Zones");
                });
#pragma warning restore 612, 618
        }
    }
}
