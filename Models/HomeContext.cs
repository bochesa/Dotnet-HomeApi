using Microsoft.EntityFrameworkCore;

namespace HomeApi.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions<HomeContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
    }
}