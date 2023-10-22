using Microsoft.EntityFrameworkCore;
using ShoppingCart.BLL;
using ShoppingCart.Data;
using ShoppingCart.Data.Repos;
using ShoppingCart.Data.SeedData;
using ShoppingCart.Models;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<EcommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceContext") ?? throw new InvalidOperationException("Connection string 'EcommerceContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<Products, Guid>), typeof(ProductsRepo));
builder.Services.AddScoped(typeof(IRepository<Country, int>), typeof(CountryRepo));
builder.Services.AddScoped(typeof(IRepository<Cart, int>), typeof(CartRepo));
builder.Services.AddScoped(typeof(IRepository<Order, int>), typeof(OrderRepo));
builder.Services.AddTransient<ProductBLL>();
builder.Services.AddTransient<CartBLL>();
builder.Services.AddTransient<OrderBLL>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await SeedData.Initialize(services);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();










