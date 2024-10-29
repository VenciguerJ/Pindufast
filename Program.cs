using PinduFast.Models;
using Microsoft.Data.SqlClient;
using PinduFast.Repositories;
using System.Data;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

string connectionString = @"Server=localhost;Database=Pindufast;Integrated Security=True;
                        TrustServerCertificate=True;";

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionString));
builder.Services.AddScoped<IRepository<Carro>, CarroRepository>();

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
