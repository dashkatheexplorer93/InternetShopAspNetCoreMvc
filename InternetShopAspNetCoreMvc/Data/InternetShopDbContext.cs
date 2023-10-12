using InternetShopAspNetCoreMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Data
{
    public class InternetShopDbContext : DbContext
	{
		public InternetShopDbContext(DbContextOptions<InternetShopDbContext> options) : base(options) { }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(InternetShopDbContext).Assembly);
			new DbInitializer(modelBuilder).Seed();
		}
	}
}
