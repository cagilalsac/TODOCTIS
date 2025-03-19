using Microsoft.EntityFrameworkCore;

namespace APP.TODO.Domain
{
    public class TodoDb : DbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoTopic> TodoTopics { get; set; }
        public DbSet<Category> Categories { get; set; }

        public TodoDb(DbContextOptions options) : base(options)
        {
        }
    }
}
