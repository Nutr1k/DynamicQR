using System;
using System.Collections.Generic;
using DynamicQR.Data.Types;
using Microsoft.EntityFrameworkCore;

namespace DynamicQR.Data;

public partial class DynamicQrContext : DbContext
{
    public DynamicQrContext()
    {
    }

    public DynamicQrContext(DbContextOptions<DynamicQrContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FileQr> FileQrs { get; set; }

    public virtual DbSet<Qr> Qrs { get; set; }

    public virtual DbSet<TypeQr> TypeQrs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileQr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Files");

            entity.ToTable("FileQR");

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.FileQrs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FileQR_Users");
        });

        modelBuilder.Entity<Qr>(entity =>
        {
            entity.ToTable("QR");

            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Qrs)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QR_TypeQR");

            entity.HasOne(d => d.User).WithMany(p => p.Qrs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QR_Users");
        });

        modelBuilder.Entity<TypeQr>(entity =>
        {
            entity.ToTable("TypeQR");

            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(15);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
