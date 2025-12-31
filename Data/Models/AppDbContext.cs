using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebsiteFirstDraft.Data.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ExerciseType> exercise_types => Set<ExerciseType>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExerciseType>(entity =>
            {
                entity.ToTable("exercise_types"); // Table name


                //Binds the class attributes to the database columns
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("exercise_id");

                entity.Property(e => e.ExerciseNames)
                      .HasColumnName("exercise_name");

                entity.Property(e => e.ExerciseTypes)
                      .HasColumnName("exercise_type");

                entity.Property(e => e.CaloriesBurnedPerMinute)
                      .HasColumnName("calories_burnt_per_minute");

                entity.Property(e => e.IntensityLevel)
                        .HasColumnName("intensity");
            });
        }

    }
}
