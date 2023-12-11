using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Domain.Common;
using green_craze_be_v1.Domain.Entities;
using green_craze_be_v1.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Infrastructure.Data.Context
{
    public class AppDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public AppDBContext(DbContextOptions options, ICurrentUserService currentUserService, IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                string tableName = type.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    type.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppUserConfiguration).Assembly);
        }

        private void SetAuditable<T>(EntityEntry<BaseAuditableEntity<T>> entry)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService?.UserId ?? "System";
                    entry.Entity.CreatedAt = _dateTimeService.Current;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService?.UserId ?? "System";
                    entry.Entity.UpdatedAt = _dateTimeService.Current;
                    break;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry<BaseAuditableEntity<long>> entry in ChangeTracker.Entries<BaseAuditableEntity<long>>())
            {
                SetAuditable(entry);
            }
            foreach (EntityEntry<BaseAuditableEntity<string>> entry in ChangeTracker.Entries<BaseAuditableEntity<string>>())
            {
                SetAuditable(entry);
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<AppUserToken> AppTokens { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Docket> Dockets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCancellationReason> OrderCancellationReasons { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UserFollowProduct> UserFollowProducts { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Ward> Wards { get; set; }
    }
}