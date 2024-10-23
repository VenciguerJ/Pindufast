using Microsoft.EntityFrameworkCore;
using PinduFast.Models;

namespace PinduFast.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Carro> Carro { get; set; }
    }
}
