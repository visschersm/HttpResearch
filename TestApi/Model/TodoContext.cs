using Microsoft.EntityFrameworkCore;
using MTech.HttpResearch.Entities;

namespace MTech.HttpResearch.DataModel
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext()
            : base()
        {
            
        }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TodoContext");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}