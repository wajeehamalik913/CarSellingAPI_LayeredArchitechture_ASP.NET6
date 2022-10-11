using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CarSellingAPI.DAL.Models;
using CarSellingAPI.DAL.Data.Models;

namespace CarSellingAPI.DAL
{
    public partial class CardbContext : IdentityDbContext<User>
    {
        public CardbContext(DbContextOptions<CardbContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Cars> Cars { get; set; } = null!;
        public virtual DbSet<Booking> Booking { get; set; } = null!;
        public virtual DbSet<Transaction> Transaction { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4"); 
            
            modelBuilder.Entity<User>(entity =>
            {
                    entity.ToTable("users");

                    entity.Property(e => e.Id)
                        .HasMaxLength(255)
                        .HasColumnName("user_id");

                    entity.Property(e => e.Email)
                        .HasMaxLength(255)
                        .HasColumnName("email");

                    entity.Property(e => e.UserName)
                        .HasMaxLength(255)
                        .HasColumnName("username");

                    entity.Property(e => e.PasswordHash)
                        .HasMaxLength(255)
                        .HasColumnName("PasswordHash");


                });

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.ToTable("Cars");

                entity.Property(e => e.CarId)
                    .HasColumnType("int(20)")
                    .HasColumnName("CarId");



                entity.Property(e => e.CarName)
                    .HasMaxLength(255)
                    .HasColumnName("CarName");

                entity.Property(e => e.BrandName)
                    .HasMaxLength(255)
                    .HasColumnName("BrandName");

                entity.Property(e => e.Price)
                    .HasColumnType("int(20)")
                    .HasColumnName("Price");

                entity.Property(e => e.MinDeposit)
                    .HasColumnType("int(20)")
                    .HasColumnName("MinDeposit");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("Status");

                entity.Property(e => e.BuyerId)
                   .HasMaxLength(255)
                   .HasColumnName("BuyerId");
            });
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.Property(e => e.BookingId)
                    .HasColumnType("int(20)")
                    .HasColumnName("BookingId");

                entity.Property(e => e.CarId)
                    .HasColumnType("int(20)")
                    .HasColumnName("carId");


                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .HasColumnName("UserId");

                entity.Property(e => e.Deposit)
                    .HasColumnType("int(20)")
                    .HasColumnName("Deposit");

                 entity.Property(e => e.AmountLeft)
                    .HasColumnType("int(20)")
                    .HasColumnName("AmountLeft");

                


            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.TranId)
                    .HasColumnType("int(20)")
                    .HasColumnName("TranId");

                entity.Property(e => e.CarId)
                    .HasColumnType("int(20)")
                    .HasColumnName("carId");

                entity.Property(e => e.UserId)
                    .HasMaxLength(255)
                    .HasColumnName("UserId");

                entity.Property(e => e.Payment)
                    .HasColumnType("int(20)")
                    .HasColumnName("Payment");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(255)
                    .HasColumnName("PaymentType");


            });

            OnModelCreatingPartial(modelBuilder);

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
