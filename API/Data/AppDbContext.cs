using Microsoft.EntityFrameworkCore;
using ModelsShared.Models;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity => 
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            modelBuilder.Entity<UsersClass>().HasKey(table => new {
                table.ClassId,
                table.UserId
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<UsersClass> UsersClass { get; set; }
        public DbSet<QuizList> QuizList { get; set; }
        public DbSet<Quiz> Quiz { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<StudentAnswersQustions> StudentAnswersQustions { get; set; }
    }

}
