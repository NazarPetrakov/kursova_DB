using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace kursova.Data;

public partial class KursovaContext : DbContext
{
    public KursovaContext()
    {
    }

    public KursovaContext(DbContextOptions<KursovaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Citizen> Citizens { get; set; }

    public virtual DbSet<Specialist> Specialists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\LocalDB;Database=kursova;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC27E48EA658");

            entity.ToTable("Application");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CitizenId).HasColumnName("CitizenID");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SpecialistId).HasColumnName("SpecialistID");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('Not accepted')");

            entity.HasOne(d => d.Citizen).WithMany(p => p.Applications)
                .HasForeignKey(d => d.CitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Applicati__Citiz__440B1D61");

            entity.HasOne(d => d.Specialist).WithMany(p => p.Applications)
                .HasForeignKey(d => d.SpecialistId)
                .HasConstraintName("FK__Applicati__Speci__44FF419A");
        });

        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Citizen__3214EC27872CB883");

            entity.ToTable("Citizen");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Specialist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speciali__3214EC278B73882C");

            entity.ToTable("Specialist");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(25);
            entity.Property(e => e.LastName).HasMaxLength(25);
            entity.Property(e => e.Specialty).HasMaxLength(75);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
