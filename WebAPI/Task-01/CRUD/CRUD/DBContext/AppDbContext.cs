using System;
using System.Collections.Generic;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.DBContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MAMUN\\SQLEXPRESS;Initial Catalog=dbERP;Connect Timeout=30;Encrypt=False;Trusted_Connection=true;TrustServerCertificate=False;ApplicationIntent=ReadWrite;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.IntOrderId);

            entity.ToTable("tblOrders");

            entity.Property(e => e.IntOrderId)
                .ValueGeneratedNever()
                .HasColumnName("intOrderId");
            entity.Property(e => e.DteLastActionDateTime)
                .HasColumnType("datetime")
                .HasColumnName("dteLastActionDateTime");
            entity.Property(e => e.DteOrderDateTime)
                .HasColumnType("datetime")
                .HasColumnName("dteOrderDateTime");
            entity.Property(e => e.IntProductId).HasColumnName("intProductId");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.NumQuantity)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("numQuantity");
            entity.Property(e => e.StrCustomerName)
                .HasMaxLength(50)
                .HasColumnName("strCustomerName");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.IntProductId);

            entity.ToTable("tblProducts");

            entity.Property(e => e.IntProductId)
                .ValueGeneratedNever()
                .HasColumnName("intProductId");
            entity.Property(e => e.NumStock)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("numStock");
            entity.Property(e => e.NumUnitPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("numUnitPrice");
            entity.Property(e => e.StrProductName)
                .HasMaxLength(50)
                .HasColumnName("strProductName");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
