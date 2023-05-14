using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Clinic_DbModel.Models
{
    public partial class clinic_dbContext : DbContext
    {
        public clinic_dbContext()
        {
        }

        public clinic_dbContext(DbContextOptions<clinic_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Scheduletiming> Scheduletimings { get; set; }
        public virtual DbSet<Socialmediaurl> Socialmediaurls { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=localhost;port=3306;user=root;password=0000;database=clinic_db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("appointment");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "fk_appointment_doctor_idx");

                entity.HasIndex(e => e.PatientId, "fk_appointment_patient_idx");

                entity.Property(e => e.Day)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.DoctorId).HasColumnName("Doctor_Id");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("tinyint")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PatientId).HasColumnName("Patient_Id");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_appointment_doctor");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_appointment_patient");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blog");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "fk_blog_doctor_idx");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.DoctorId).HasColumnName("Doctor_Id");

                entity.Property(e => e.Image).HasMaxLength(45);

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("tinyint")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_blog_doctor");
            });

            modelBuilder.Entity<Clinic>(entity =>
            {
                entity.ToTable("clinic");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "fk_clinic_doctor_idx");

                entity.Property(e => e.ClinicName)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.DoctorId).HasColumnName("Doctor_Id");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Clinics)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_clinic_doctor");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctor");

                entity.HasIndex(e => e.Email, "Email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.SpecialtyId, "fk_doctor_specialty_idx");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.College).HasMaxLength(255);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(45);

                entity.Property(e => e.Degree).HasMaxLength(255);

                entity.Property(e => e.DoctorServices).HasMaxLength(255);

                entity.Property(e => e.DoctorSpecialization).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Gender).HasMaxLength(45);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(255);

                entity.Property(e => e.SpecialtyId).HasColumnName("Specialty_Id");

                entity.Property(e => e.Username).HasMaxLength(45);

                entity.Property(e => e.YearOfCompletion).HasMaxLength(255);

                entity.HasOne(d => d.Specialty)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.SpecialtyId)
                    .HasConstraintName("fk_doctor_specialization");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.Property(e => e.BloodGroup).HasMaxLength(255);

                entity.Property(e => e.City).HasMaxLength(255);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StreetAddress).HasMaxLength(255);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PatientId, "fk_review_patient_idx");

                entity.Property(e => e.PatientId).HasColumnName("Patient_Id");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_review_patient");
            });

            modelBuilder.Entity<Scheduletiming>(entity =>
            {
                entity.ToTable("scheduletimings");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "fk_scheduletiming_doctor_idx");

                entity.Property(e => e.Day).HasMaxLength(45);

                entity.Property(e => e.DoctorId).HasColumnName("Doctor_Id");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Scheduletimings)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_scheduletiming_doctor");
            });

            modelBuilder.Entity<Socialmediaurl>(entity =>
            {
                entity.ToTable("socialmediaurl");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.DoctorId, "fk_socialmediaurl_doctor_idx");

                entity.Property(e => e.DoctorId).HasColumnName("Doctor_Id");

                entity.Property(e => e.FacebookUrl)
                    .HasMaxLength(255)
                    .HasColumnName("FacebookURL");

                entity.Property(e => e.InstagramUrl)
                    .HasMaxLength(255)
                    .HasColumnName("InstagramURL");

                entity.Property(e => e.LinkedInUrl)
                    .HasMaxLength(255)
                    .HasColumnName("LinkedInURL");

                entity.Property(e => e.TwitterUrl)
                    .HasMaxLength(255)
                    .HasColumnName("TwitterURL");

                entity.Property(e => e.WebsiteUrl)
                    .HasMaxLength(255)
                    .HasColumnName("WebsiteURL");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Socialmediaurls)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_socialmediaurl_doctor");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("specialization");

                entity.HasIndex(e => e.Id, "Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IsDelete)
                    .HasColumnType("tinyint")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SpecialtyName)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
