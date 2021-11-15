using Microsoft.EntityFrameworkCore;

namespace HomeApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext() { }
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
    }
}