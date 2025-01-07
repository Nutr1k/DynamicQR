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

    public virtual DbSet<Qr> Qrs { get; set; }

    public virtual DbSet<TypeQr> TypeQrs { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Qr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("QR");

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithMany()
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QR_Users");

            entity.HasOne(d => d.TypeNavigation).WithMany()
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QR_TypeQR");
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
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
