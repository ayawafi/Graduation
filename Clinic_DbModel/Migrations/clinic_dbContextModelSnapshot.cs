﻿// <auto-generated />
using System;
using Clinic_DbModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Clinic_DbModel.Migrations
{
    [DbContext(typeof(clinic_dbContext))]
    partial class clinic_dbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Clinic_DbModel.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("BloodGroup")
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<int>("ConfirmationCode")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserType")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("Doctor_Id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<sbyte>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValueSql("'0'");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("Status")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("User_Id");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique();

                    b.HasIndex(new[] { "DoctorId" }, "fk_appointment_doctor_idx");

                    b.HasIndex(new[] { "UserId" }, "fk_appointment_user_idx");

                    b.ToTable("appointment", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("Doctor_Id");

                    b.Property<string>("Image")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<sbyte?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE1");

                    b.HasIndex(new[] { "DoctorId" }, "fk_blog_doctor_idx");

                    b.ToTable("blog", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AboutMe")
                        .HasColumnType("longtext");

                    b.Property<string>("Awards")
                        .HasColumnType("longtext");

                    b.Property<int?>("AwardsYear")
                        .HasColumnType("int");

                    b.Property<string>("ClinicAddress")
                        .HasColumnType("longtext");

                    b.Property<string>("ClinicName")
                        .HasColumnType("longtext");

                    b.Property<string>("College")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Degree")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Designation")
                        .HasColumnType("longtext");

                    b.Property<string>("DoctorServices")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("HospitalFrom")
                        .HasColumnType("int");

                    b.Property<string>("HospitalName")
                        .HasColumnType("longtext");

                    b.Property<int?>("HospitalTo")
                        .HasColumnType("int");

                    b.Property<string>("Membership")
                        .HasColumnType("longtext");

                    b.Property<string>("Pricing")
                        .HasColumnType("longtext");

                    b.Property<string>("Registration")
                        .HasColumnType("longtext");

                    b.Property<int?>("RegistrationYear")
                        .HasColumnType("int");

                    b.Property<int?>("SpecialtyId")
                        .HasColumnType("int")
                        .HasColumnName("Specialty_Id");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("YearOfCompletion")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE2");

                    b.HasIndex(new[] { "SpecialtyId" }, "fk_doctor_specialty_idx");

                    b.ToTable("doctor", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.FavDoctors", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("DoctorId", "ApplicationUserId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("FavDoctors");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("User_Id");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE3");

                    b.HasIndex(new[] { "UserId" }, "fk_review_user_idx");

                    b.ToTable("review", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Scheduletiming", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Day")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("Doctor_Id");

                    b.Property<int>("DurationTime")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE4");

                    b.HasIndex(new[] { "DoctorId" }, "fk_scheduletiming_doctor_idx");

                    b.ToTable("scheduletimings", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Socialmediaurl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("Doctor_Id");

                    b.Property<string>("FacebookUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("FacebookURL");

                    b.Property<string>("InstagramUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("InstagramURL");

                    b.Property<string>("LinkedInUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("LinkedInURL");

                    b.Property<string>("TwitterUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("TwitterURL");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("WebsiteURL");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE5");

                    b.HasIndex(new[] { "DoctorId" }, "fk_socialmediaurl_doctor_idx");

                    b.ToTable("socialmediaurl", (string)null);
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<sbyte>("IsDelete")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValueSql("'0'");

                    b.Property<string>("SpecialtyName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique()
                        .HasDatabaseName("Id_UNIQUE6");

                    b.ToTable("specialization", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("longtext");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("RoleId")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Appointment", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Doctor", "Doctor")
                        .WithMany("Appointments")
                        .HasForeignKey("DoctorId")
                        .IsRequired()
                        .HasConstraintName("fk_appointment_doctor");

                    b.HasOne("Clinic_DbModel.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_appointment_patient");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Blog", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Doctor", "Doctor")
                        .WithMany("Blogs")
                        .HasForeignKey("DoctorId")
                        .IsRequired()
                        .HasConstraintName("fk_blog_doctor");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Doctor", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Specialization", "Specialty")
                        .WithMany("Doctors")
                        .HasForeignKey("SpecialtyId")
                        .HasConstraintName("fk_doctor_specialization");

                    b.HasOne("Clinic_DbModel.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.FavDoctors", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clinic_DbModel.Models.Doctor", "Dcotor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Dcotor");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Review", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Doctor", "Doctors")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Clinic_DbModel.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_review_user");

                    b.Navigation("ApplicationUser");

                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Scheduletiming", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Doctor", "Doctor")
                        .WithMany("Scheduletimings")
                        .HasForeignKey("DoctorId")
                        .IsRequired()
                        .HasConstraintName("fk_scheduletiming_doctor");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Socialmediaurl", b =>
                {
                    b.HasOne("Clinic_DbModel.Models.Doctor", "Doctor")
                        .WithMany("Socialmediaurls")
                        .HasForeignKey("DoctorId")
                        .IsRequired()
                        .HasConstraintName("fk_socialmediaurl_doctor");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.ApplicationUser", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Doctor", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Blogs");

                    b.Navigation("Scheduletimings");

                    b.Navigation("Socialmediaurls");
                });

            modelBuilder.Entity("Clinic_DbModel.Models.Specialization", b =>
                {
                    b.Navigation("Doctors");
                });
#pragma warning restore 612, 618
        }
    }
}
