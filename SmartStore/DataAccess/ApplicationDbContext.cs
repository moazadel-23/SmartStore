using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SmartStore.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImges> ProductImgs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<BannerModel> Banners { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.ProductId });
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId);

            modelBuilder.Entity<Cart>()
                .HasKey(f => new { f.UserId, f.ProductId });
            modelBuilder.Entity<Cart>()
                .HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId);
            modelBuilder.Entity<Cart>()
                .HasOne(f => f.applicationUser)
                .WithMany()
                .HasForeignKey(f => f.UserId);
        }
    }
}
