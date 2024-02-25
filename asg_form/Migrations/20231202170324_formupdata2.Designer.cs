﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using asg_form.Controllers;

#nullable disable

namespace asg_form.Migrations
{
    [DbContext(typeof(IDBcontext))]
    [Migration("20231202170324_formupdata2")]
    partial class formupdata2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.Champion+T_Champion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("eventsId")
                        .HasColumnType("int");

                    b.Property<long>("formId")
                        .HasColumnType("bigint");

                    b.Property<string>("msg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("eventsId");

                    b.HasIndex("formId");

                    b.ToTable("F_Champion", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.Events+T_events", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("events_rule_uri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("is_over")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("opentime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("F_events", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.T_news", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("FormName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("msg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("time")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("F_news", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.comform+com_form", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Com_Cocial_media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Com_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Com_qq")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("idv_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("introduction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("F_ComForm", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.form", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("eventsId")
                        .HasColumnType("int");

                    b.Property<string>("logo_uri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("piaoshu")
                        .HasColumnType("int");

                    b.Property<string>("team_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("team_password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("team_tel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("eventsId");

                    b.ToTable("F_form", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Common_Roles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Game_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Historical_Ranks")
                        .HasColumnType("int");

                    b.Property<string>("Id_Card")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Id_Card_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone_Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("formId")
                        .HasColumnType("bigint");

                    b.Property<string>("role_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role_lin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("formId");

                    b.ToTable("F_role", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.schedule+schedule_log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("chickteam")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("teamid")
                        .HasColumnType("bigint");

                    b.Property<string>("userid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("win")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("teamid");

                    b.ToTable("F_achlog", (string)null);
                });

            modelBuilder.Entity("asg_form.Controllers.schedule+team_game", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("id"));

                    b.Property<string>("belong")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("bilibiliuri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("commentary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("opentime")
                        .HasColumnType("datetime2");

                    b.Property<string>("referee")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("team1_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("team1_piaoshu")
                        .HasColumnType("int");

                    b.Property<string>("team2_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("team2_piaoshu")
                        .HasColumnType("int");

                    b.Property<string>("winteam")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("F_game", (string)null);
                });

            modelBuilder.Entity("asg_form.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("msg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("asg_form.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("chinaname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("haveformId")
                        .HasColumnType("bigint");

                    b.Property<bool?>("isbooking")
                        .HasColumnType("bit");

                    b.Property<string>("officium")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("haveformId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("asg_form.blog+blog_db", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("formuser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("msg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("pushtime")
                        .HasColumnType("datetime2");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("F_blog", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("asg_form.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("asg_form.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("asg_form.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("asg_form.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("asg_form.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("asg_form.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("asg_form.Controllers.Champion+T_Champion", b =>
                {
                    b.HasOne("asg_form.Controllers.Events+T_events", "events")
                        .WithMany()
                        .HasForeignKey("eventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("asg_form.Controllers.form", "form")
                        .WithMany()
                        .HasForeignKey("formId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("events");

                    b.Navigation("form");
                });

            modelBuilder.Entity("asg_form.Controllers.form", b =>
                {
                    b.HasOne("asg_form.Controllers.Events+T_events", "events")
                        .WithMany("forms")
                        .HasForeignKey("eventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("events");
                });

            modelBuilder.Entity("asg_form.Controllers.role", b =>
                {
                    b.HasOne("asg_form.Controllers.form", "form")
                        .WithMany("role")
                        .HasForeignKey("formId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("form");
                });

            modelBuilder.Entity("asg_form.Controllers.schedule+schedule_log", b =>
                {
                    b.HasOne("asg_form.Controllers.schedule+team_game", "team")
                        .WithMany("logs")
                        .HasForeignKey("teamid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("team");
                });

            modelBuilder.Entity("asg_form.User", b =>
                {
                    b.HasOne("asg_form.Controllers.form", "haveform")
                        .WithMany()
                        .HasForeignKey("haveformId");

                    b.Navigation("haveform");
                });

            modelBuilder.Entity("asg_form.Controllers.Events+T_events", b =>
                {
                    b.Navigation("forms");
                });

            modelBuilder.Entity("asg_form.Controllers.form", b =>
                {
                    b.Navigation("role");
                });

            modelBuilder.Entity("asg_form.Controllers.schedule+team_game", b =>
                {
                    b.Navigation("logs");
                });
#pragma warning restore 612, 618
        }
    }
}
