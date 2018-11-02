﻿// <auto-generated />
using System;
using GoldenLadle.Data;
using GoldenLadle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GoldenLadle.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180322145759_AddThumbToFilePath")]
    partial class AddThumbToFilePath
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("GoldenLadle.Models.Entry", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreateDate");

                b.Property<DateTime>("DeletedDate");

                b.Property<string>("Description");

                b.Property<int>("EventId");

                b.Property<DateTime>("ModifiedDate");

                b.Property<string>("Name");

                b.Property<string>("UserId");

                b.Property<int>("Value");

                b.HasKey("Id");

                b.HasIndex("EventId");

                b.HasIndex("UserId");

                b.ToTable("Entries");
            });

            modelBuilder.Entity("GoldenLadle.Models.Event", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreateDate");

                b.Property<DateTime>("DeletedDate");

                b.Property<string>("Description")
                    .IsRequired();

                b.Property<DateTime>("EndDT");

                b.Property<DateTime>("ModifiedDate");

                b.Property<string>("Name")
                    .IsRequired();

                b.Property<DateTime>("StartDT");

                b.HasKey("Id");

                b.ToTable("Events");
            });

            modelBuilder.Entity("GoldenLadle.Models.FilePath", b =>
            {
                b.Property<int>("FilePathId")
                    .ValueGeneratedOnAdd();

                b.Property<int>("EventId");

                b.Property<string>("FileName")
                    .HasMaxLength(255);

                b.Property<int>("FileType");

                b.Property<string>("ThumbName");

                b.HasKey("FilePathId");

                b.HasIndex("EventId");

                b.ToTable("FilePaths");
            });

            modelBuilder.Entity("GoldenLadle.Models.User", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Email")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

                b.Property<string>("FirstName");

                b.Property<string>("LastName");

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash");

                b.Property<string>("PhoneNumber");

                b.Property<bool>("PhoneNumberConfirmed");

                b.Property<string>("SecurityStamp");

                b.Property<bool>("TwoFactorEnabled");

                b.Property<string>("UserName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasName("UserNameIndex");

                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity("GoldenLadle.Models.Vote", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreateDate");

                b.Property<DateTime>("DeletedDate");

                b.Property<int>("EntryId");

                b.Property<int>("EventId");

                b.Property<DateTime>("ModifiedDate");

                b.Property<string>("UserId");

                b.Property<byte>("Value");

                b.HasKey("Id");

                b.HasIndex("EntryId");

                b.HasIndex("EventId");

                b.HasIndex("UserId");

                b.ToTable("Vote");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken();

                b.Property<string>("Name")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasName("RoleNameIndex");

                b.ToTable("AspNetRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("RoleId")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("UserId")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider");

                b.Property<string>("ProviderKey");

                b.Property<string>("ProviderDisplayName");

                b.Property<string>("UserId")
                    .IsRequired();

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId");

                b.Property<string>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId");

                b.Property<string>("LoginProvider");

                b.Property<string>("Name");

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            modelBuilder.Entity("GoldenLadle.Models.Entry", b =>
            {
                b.HasOne("GoldenLadle.Models.Event", "Event")
                    .WithMany("Entries")
                    .HasForeignKey("EventId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GoldenLadle.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId");
            });

            modelBuilder.Entity("GoldenLadle.Models.FilePath", b =>
            {
                b.HasOne("GoldenLadle.Models.Event", "Event")
                    .WithMany("FilePaths")
                    .HasForeignKey("EventId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("GoldenLadle.Models.Vote", b =>
            {
                b.HasOne("GoldenLadle.Models.Entry", "Entry")
                    .WithMany("Votes")
                    .HasForeignKey("EntryId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GoldenLadle.Models.Event", "Event")
                    .WithMany()
                    .HasForeignKey("EventId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GoldenLadle.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("GoldenLadle.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("GoldenLadle.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("GoldenLadle.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("GoldenLadle.Models.User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }
    }
}