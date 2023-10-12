using InternetShopAspNetCoreMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetShopAspNetCoreMvc.Data
{
	public class DbInitializer
	{
		private readonly ModelBuilder _modelBuilder;

		public DbInitializer(ModelBuilder modelBuilder)
		{
			_modelBuilder = modelBuilder;
		}

		public void Seed()
		{
			_modelBuilder.Entity<User>(x =>
			{
				x.HasData(new User
				{
					Id = 1,
					Username = "Vasya",
					Email = "vasya@gmail.com",
					Address = "test-1",
					Fullname = "Vasya Pupkin",
					CreatedAt = DateTime.UtcNow
				});
				x.HasData(new User
				{
					Id = 2,
					Username = "Petya",
					Email = "petya@gmail.com",
					Address = "test-2",
					Fullname = "Petya Pupkin",
					CreatedAt = DateTime.UtcNow
				});
			});
		}
	}
}
