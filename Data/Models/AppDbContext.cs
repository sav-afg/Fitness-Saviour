using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebsiteFirstDraft.Data.DatabaseTableModels;

namespace WebsiteFirstDraft.Data.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ExerciseType> exercise_types => Set<ExerciseType>();

        public DbSet<User> Users => Set<User>();

        // Creates the new database table
        public DbSet<CalorieLogs> Calorie_Logs => Set<CalorieLogs>();

        public DbSet<WeightLog> Weight_Logs => Set<WeightLog>();

        public DbSet<UserExercises> users_exercises => Set<UserExercises>();

        public DbSet<UserFoodItems> users_food_items => Set<UserFoodItems>();


        // DbSet representing the food_types table in the database
        public DbSet<FoodType> Food_items => Set<FoodType>();

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

            // Configure UserFoodItems table
            modelBuilder.Entity<UserFoodItems>(entity =>
            {
                entity.ToTable("users_food_items");
                entity.HasKey(e => e.UsersFoodItemId);

                // Configure relationships
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FoodType)
                      .WithMany()
                      .HasForeignKey(e => e.FoodId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
