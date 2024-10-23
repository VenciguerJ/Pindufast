using PinduFast.Data;
using PinduFast.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost;Database=Pindufast;Integrated Security=True;TrustServerCertificate=True;"));
//builder.Services.AddScoped(sp => )

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Carro/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carro}/{action=Index}/{id?}");

app.Run();
