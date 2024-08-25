using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Models;

public partial class DatabaseServerContext : DbContext
{
    public DatabaseServerContext()
    {
    }

    public DatabaseServerContext(DbContextOptions<DatabaseServerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventGuide> EventGuides { get; set; }

    public virtual DbSet<EventMember> EventMembers { get; set; }

    public virtual DbSet<Guide> Guides { get; set; }

    public virtual DbSet<Lookup> Lookups { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
        if (!optionsBuilder.IsConfigured)
        {
            // This should use the 'Name=' syntax to refer to the named connection string in your appsettings.json
            optionsBuilder.UseSqlServer("Name=MyDatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Admins__CB9A1CFF452B2997");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__Admins__userId__5441852A");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3213E83F45BB20AF");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.DateFrom).HasColumnName("dateFrom");
            entity.Property(e => e.DateTo).HasColumnName("dateTo");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Destination)
                .HasMaxLength(200)
                .HasColumnName("destination");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(200)
                .HasColumnName("status");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull) // Enable Cascade Set Null
                .HasConstraintName("FK__Events__category__59063A47");
        });

        modelBuilder.Entity<EventGuide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventGui__3213E83FB2E75A86");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.GuideId).HasColumnName("guideId");

            entity.HasOne(d => d.Event).WithMany(p => p.EventGuides)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__EventGuid__event__6383C8BA");

            entity.HasOne(d => d.Guide).WithMany(p => p.EventGuides)
                .HasForeignKey(d => d.GuideId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__EventGuid__guide__628FA481");
        });

        modelBuilder.Entity<EventMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EventMem__3213E83F00B681DC");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.MemberId).HasColumnName("memberId");

            entity.HasOne(d => d.Event).WithMany(p => p.EventMembers)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__EventMemb__event__5FB337D6");

            entity.HasOne(d => d.Member).WithMany(p => p.EventMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__EventMemb__membe__5EBF139D");
        });

        modelBuilder.Entity<Guide>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Guides__CB9A1CFF2A703E41");

            entity.HasIndex(e => e.Profession, "UQ__Guides__F4EB62589A0C1545").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");
            entity.Property(e => e.JoiningDate).HasColumnName("joiningDate");
            entity.Property(e => e.Photo)
                .HasMaxLength(200)
                .HasColumnName("photo");
            entity.Property(e => e.Profession)
                .HasMaxLength(200)
                .HasColumnName("profession");

            entity.HasOne(d => d.User).WithOne(p => p.Guide)
                .HasForeignKey<Guide>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__Guides__userId__5165187F");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Members__CB9A1CFF7A44FDDF");

            entity.HasIndex(e => e.Profession, "UQ__Members__F4EB6258179240D2").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("userId");
            entity.Property(e => e.EmergencyNumber)
                .HasMaxLength(20)
                .HasColumnName("emergencyNumber");
            entity.Property(e => e.JoiningDate).HasColumnName("joiningDate");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(20)
                .HasColumnName("mobileNumber");
            entity.Property(e => e.Nationality)
                .HasMaxLength(200)
                .HasColumnName("nationality");
            entity.Property(e => e.Photo)
                .HasMaxLength(200)
                .HasColumnName("photo");
            entity.Property(e => e.Profession)
                .HasMaxLength(200)
                .HasColumnName("profession");

            entity.HasOne(d => d.User).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade) // Enable Cascade Delete
                .HasConstraintName("FK__Members__userId__4D94879B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F0573F724");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61649646C634").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .HasColumnName("fullName");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
