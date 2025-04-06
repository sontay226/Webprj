using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Webprj.Models;

namespace Webprj.Models
{
    public partial class Test2WebContext : DbContext
    {
        public Test2WebContext()
        {
        }

        public Test2WebContext(DbContextOptions<Test2WebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Discount> Discounts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Shipment> Shipments { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=SONTAY2206;Database=test_2_web;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(255);

                entity.Property(e => e.Description).HasColumnType("text");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email , "UQ__Customer__A9D1053485F331D6")
    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.BillingAddress).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerName).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.PasswordHash).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingAddress).HasMaxLength(255);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasIndex(e => e.DiscountCode, "UQ__Discount__A1120AF5026E0037")
                    .IsUnique();

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.DiscountAmount).HasColumnType("money");

                entity.Property(e => e.DiscountCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountType).HasMaxLength(10);

                entity.Property(e => e.EndDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StartDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PaymentMethod).HasMaxLength(50);

                entity.Property(e => e.ShippingAddress).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Customer__46E78A0C");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.TotalCost).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderItem__Order__4BAC3F29");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__OrderItem__Produ__4CA06362");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PayMethod).HasMaxLength(50);

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Payments__OrderI__5165187F");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Certificate).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("ImageURL");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.TechnicalSpecifications).HasColumnType("text");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__Catego__3E52440B");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Products__Suppli__3F466844");

                entity.HasMany(d => d.Discounts)
                    .WithMany(p => p.Products)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProductDiscount",
                        l => l.HasOne<Discount>().WithMany().HasForeignKey("DiscountId").HasConstraintName("FK__Product_D__Disco__6A30C649"),
                        r => r.HasOne<Product>().WithMany().HasForeignKey("ProductId").HasConstraintName("FK__Product_D__Produ__693CA210"),
                        j =>
                        {
                            j.HasKey("ProductId", "DiscountId").HasName("PK__Product___CA4F3032F0D1942F");

                            j.ToTable("Product_Discounts");

                            j.IndexerProperty<int>("ProductId").HasColumnName("ProductID");

                            j.IndexerProperty<int>("DiscountId").HasColumnName("DiscountID");
                        });
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShipperName).HasMaxLength(100);

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Shipments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__Shipments__Order__5629CD9C");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Supplier__A9D105343E0D519B")
                    .IsUnique();

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Address).HasColumnType("text");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortName).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
