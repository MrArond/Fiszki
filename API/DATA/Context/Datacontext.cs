using API.DATA.Models;
using Microsoft.EntityFrameworkCore;
namespace API.DATA.Context
{
    public class Datacontext : DbContext
    {
        public Datacontext(DbContextOptions<Datacontext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<FlashCardsLists> FlashCardsLists { get; set; }
        public DbSet<FlashCards> FlashCards { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

}
