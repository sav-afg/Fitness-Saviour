using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebsiteFirstDraft.Data.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ExerciseType> ExerciseTypes => Set<ExerciseType>();
    }
}
