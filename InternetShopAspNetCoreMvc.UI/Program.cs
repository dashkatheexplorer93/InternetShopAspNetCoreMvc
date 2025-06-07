using InternetShopAspNetCoreMvc.Data;
using InternetShopAspNetCoreMvc.Data.Interfaces;
using InternetShopAspNetCoreMvc.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using InternetShopAspNetCoreMvc.UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddDbContext<InternetShopDbContext>(options => 
								options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // add 

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<InternetShopDbContext>();
	dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
//if (!app.Environment.IsDevelopment())
//{
//	//app.UseExceptionHandler("/Products/Error");
//	app.UseDeveloperExceptionPage();
//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//	app.UseHsts();
//}
//else
//{
//	app.UseDeveloperExceptionPage();
//}

app.Use(async (context, next) =>
{
	await next();
	if (context.Response.StatusCode == 404)
	{
		context.Request.Path = "/PageNotFound";
		await next();
	}
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Products}/{action=Index}");

app.UseNotyf();

app.Run();
