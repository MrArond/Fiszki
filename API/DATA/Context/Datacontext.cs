using Microsoft.EntityFrameworkCore;
using API.DATA.Models;
namespace API.DATA.Context
{
    public class Datacontext : DbContext
    {
        public Datacontext(DbContextOptions<Datacontext> options) : base(options) 
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Host=localhost;Port=5432;Database=flashcards;Username=postgres;Password=zaq1@WSX;";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
    
}
 