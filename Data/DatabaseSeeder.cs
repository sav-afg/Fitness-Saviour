using WebsiteFirstDraft.Data.DatabaseTableModels;
using WebsiteFirstDraft.Data.Models;

namespace WebsiteFirstDraft.Data
{
    public class DatabaseSeeder
    {
        public static void SeedDatabase(AppDbContext context)
        {
            // Check if database is already seeded
            if (context.Users.Any())
            {
                Console.WriteLine("? Database already seeded. Skipping seed operation.");
                Console.WriteLine($"?? Current user count: {context.Users.Count()}");
                return; // Database already seeded
            }

            Console.WriteLine("?? Starting database seeding...");

            // Create 3 users with hashed passwords
            var users = new List<User>
            {
                new User
                {
                    User_id = 1,
                    Username = "john_fitness",
                    PasswordHash = PasswordHelper.HashPassword("Password123!"),
                    Email = "john@example.com",
                    Phone_Number = "555-0001",
                    Role = "User",
                    Login_Streak = 15,
                    Body_Weight = 85,
                    Maintenance_Calories = 2500,
                    Daily_Calories = 2650,
                    Daily_Carbs = 300,
                    Daily_Protein = 150,
                    Daily_Fat = 80,
                    Weekly_Calories = 17800,
                    Weekly_Carbs = 2100,
                    Weekly_Protein = 1050,
                    Weekly_Fat = 560,
                    Total_Calories = 52000,
                    High_Contrast_Mode = false,
                    Dyslexia_Friendly_Font = false,
                    Reduced_Animations = false,
                    Larger_Font_Size = false,
                    Tracking_Preferences = "All",
                    Visual_Rewards = true,
                    Progress_Data = true,
                    Minimal_Interface = false,
                    Daily_Weight_Change = -0.2,
                    Weekly_Weight_Change = -0.8,
                    Daily_Calories_Burnt_Through_Exercise = 450,
                    Weekly_Calories_Burnt_Through_Exercise = 2700,
                    Daily_Cardio = 30,
                    Daily_Strength = 45,
                    Daily_Flexibility = 15,
                    Created_At = DateTime.UtcNow.AddMonths(-3)
                },
                new User
                {
                    User_id = 2,
                    Username = "sarah_health",
                    PasswordHash = PasswordHelper.HashPassword("Password123!"),
                    Email = "sarah@example.com",
                    Phone_Number = "555-0002",
                    Role = "User",
                    Login_Streak = 30,
                    Body_Weight = 68,
                    Maintenance_Calories = 2000,
                    Daily_Calories = 1850,
                    Daily_Carbs = 200,
                    Daily_Protein = 120,
                    Daily_Fat = 65,
                    Weekly_Calories = 13300,
                    Weekly_Carbs = 1400,
                    Weekly_Protein = 840,
                    Weekly_Fat = 455,
                    Total_Calories = 45000,
                    High_Contrast_Mode = true,
                    Dyslexia_Friendly_Font = false,
                    Reduced_Animations = true,
                    Larger_Font_Size = false,
                    Tracking_Preferences = "Calories",
                    Visual_Rewards = true,
                    Progress_Data = true,
                    Minimal_Interface = false,
                    Daily_Weight_Change = 0.1,
                    Weekly_Weight_Change = 0.3,
                    Daily_Calories_Burnt_Through_Exercise = 350,
                    Weekly_Calories_Burnt_Through_Exercise = 2100,
                    Daily_Cardio = 40,
                    Daily_Strength = 30,
                    Daily_Flexibility = 20,
                    Created_At = DateTime.UtcNow.AddMonths(-6)
                },
                new User
                {
                    User_id = 3,
                    Username = "mike_athlete",
                    PasswordHash = PasswordHelper.HashPassword("Password123!"),
                    Email = "mike@example.com",
                    Phone_Number = "555-0003",
                    Role = "User",
                    Login_Streak = 45,
                    Body_Weight = 92,
                    Maintenance_Calories = 3000,
                    Daily_Calories = 3200,
                    Daily_Carbs = 400,
                    Daily_Protein = 180,
                    Daily_Fat = 90,
                    Weekly_Calories = 22400,
                    Weekly_Carbs = 2800,
                    Weekly_Protein = 1260,
                    Weekly_Fat = 630,
                    Total_Calories = 68000,
                    High_Contrast_Mode = false,
                    Dyslexia_Friendly_Font = true,
                    Reduced_Animations = false,
                    Larger_Font_Size = true,
                    Tracking_Preferences = "All",
                    Visual_Rewards = true,
                    Progress_Data = true,
                    Minimal_Interface = false,
                    Daily_Weight_Change = 0.3,
                    Weekly_Weight_Change = 1.2,
                    Daily_Calories_Burnt_Through_Exercise = 600,
                    Weekly_Calories_Burnt_Through_Exercise = 3600,
                    Daily_Cardio = 20,
                    Daily_Strength = 60,
                    Daily_Flexibility = 10,
                    Created_At = DateTime.UtcNow.AddMonths(-12)
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed Weight Logs for past 8 weeks
            SeedWeightLogs(context, users);

            // Seed Calorie Logs for past 30 days
            SeedCalorieLogs(context, users);

            // Seed Exercise Types
            SeedExerciseTypes(context);

            // Seed Food Types
            SeedFoodTypes(context);

            context.SaveChanges();

            Console.WriteLine("? Database seeding completed successfully!");
            Console.WriteLine($"   ?? Users: {context.Users.Count()}");
            Console.WriteLine($"   ??  Weight Logs: {context.Weight_Logs.Count()}");
            Console.WriteLine($"   ???  Calorie Logs: {context.Calorie_Logs.Count()}");
            Console.WriteLine($"   ?? Exercise Types: {context.exercise_types.Count()} (110 total)");
            Console.WriteLine($"   ?? Food Types: {context.Food_items.Count()} (110 total)");
            Console.WriteLine();
            Console.WriteLine("?? Login with these credentials:");
            Console.WriteLine("   • john_fitness / Password123!");
            Console.WriteLine("   • sarah_health / Password123!");
            Console.WriteLine("   • mike_athlete / Password123!");
        }

        private static void SeedWeightLogs(AppDbContext context, List<User> users)
        {
            var weightLogs = new List<WeightLog>();
            var random = new Random(42); // Fixed seed for reproducibility

            foreach (var user in users)
            {
                double baseWeight = user.Body_Weight;
                double weightTrend = user.User_id == 1 ? -0.15 : (user.User_id == 2 ? 0.05 : 0.2); // John losing, Sarah maintaining, Mike bulking

                // Generate weight logs for past 56 days (8 weeks)
                for (int daysAgo = 56; daysAgo >= 0; daysAgo--)
                {
                    var logDate = DateTime.Today.AddDays(-daysAgo);
                    
                    // Add some randomness to weight (±0.5 kg)
                    double dailyVariation = (random.NextDouble() - 0.5) * 1.0;
                    double weight = baseWeight + (weightTrend * (56 - daysAgo)) + dailyVariation;

                    // Log weight every 2-3 days (not every day for realism)
                    if (daysAgo % 3 == 0 || daysAgo % 2 == 0)
                    {
                        weightLogs.Add(new WeightLog
                        {
                            UserId = user.User_id,
                            LogDate = logDate,
                            Weight = Math.Round(weight, 1)
                        });
                    }
                }
            }

            context.Weight_Logs.AddRange(weightLogs);
        }

        private static void SeedCalorieLogs(AppDbContext context, List<User> users)
        {
            var calorieLogs = new List<CalorieLogs>();
            var random = new Random(42);

            foreach (var user in users)
            {
                int maintenanceCals = user.Maintenance_Calories;
                int avgCarbs = user.Daily_Carbs;
                int avgProtein = user.Daily_Protein;
                int avgFat = user.Daily_Fat;

                // Generate logs for past 30 days
                for (int daysAgo = 30; daysAgo >= 0; daysAgo--)
                {
                    var logDate = DateTime.Today.AddDays(-daysAgo);

                    // Vary daily intake ±300 calories
                    int caloriesConsumed = maintenanceCals + random.Next(-300, 300);
                    int caloriesBurned = user.Daily_Calories_Burnt_Through_Exercise + random.Next(-100, 100);

                    // Calculate macros (with some variation)
                    int carbs = avgCarbs + random.Next(-30, 30);
                    int protein = avgProtein + random.Next(-20, 20);
                    int fat = avgFat + random.Next(-10, 10);

                    // Create 2-4 logs per day (breakfast, lunch, dinner, snacks)
                    int mealsPerDay = random.Next(2, 5);
                    int caloriesPerMeal = caloriesConsumed / mealsPerDay;
                    int carbsPerMeal = carbs / mealsPerDay;
                    int proteinPerMeal = protein / mealsPerDay;
                    int fatPerMeal = fat / mealsPerDay;

                    for (int meal = 0; meal < mealsPerDay; meal++)
                    {
                        calorieLogs.Add(new CalorieLogs
                        {
                            User_id = user.User_id,
                            Log_Date = logDate.AddHours(6 + meal * 4), // Spread throughout the day
                            Calories_Consumed = caloriesPerMeal,
                            Calories_Burned = meal == 0 ? caloriesBurned : 0, // Only count exercise once per day
                            Net_Calories = caloriesPerMeal,
                            Carbs_Consumed = carbsPerMeal,
                            Protein_Consumed = proteinPerMeal,
                            Fat_Consumed = fatPerMeal,
                            Calories_From_Carbs = carbsPerMeal * 4,
                            Calories_From_Protein = proteinPerMeal * 4,
                            Calories_From_Fats = fatPerMeal * 9
                        });
                    }
                }
            }

            context.Calorie_Logs.AddRange(calorieLogs);
        }

        private static void SeedExerciseTypes(AppDbContext context)
        {
            var exerciseTypes = new List<ExerciseType>
            {
                // Cardio Exercises
                new ExerciseType { ExerciseNames = "Running", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Cycling", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Swimming", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 12.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Walking", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Jogging", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Sprinting", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 15.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Rowing Machine", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Elliptical", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Stair Climbing", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 10.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Jump Rope", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 13.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Mountain Climbing", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 12.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Dancing", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Boxing", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 11.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Kickboxing", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Hiking", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Basketball", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 8.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Soccer", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Tennis", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 7.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Badminton", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Racquetball", ExerciseTypes = "Cardio", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "High" },
                
                // Strength Training Exercises
                new ExerciseType { ExerciseNames = "Bench Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Squats", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Deadlifts", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Pull-ups", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Push-ups", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Dips", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Shoulder Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Bicep Curls", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Tricep Extensions", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Lat Pulldowns", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 5.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Leg Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Lunges", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Leg Curls", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Calf Raises", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 3.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Cable Flyes", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Incline Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Decline Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Romanian Deadlifts", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Front Squats", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Overhead Squats", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 7.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Bulgarian Split Squats", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Farmers Walk", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Battle Ropes", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Kettlebell Swings", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "TRX Rows", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 5.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Arnold Press", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Face Pulls", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Hammer Curls", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Preacher Curls", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Skull Crushers", ExerciseTypes = "Strength", CaloriesBurnedPerMinute = 4.5, IntensityLevel = "Medium" },
                
                // Flexibility Exercises
                new ExerciseType { ExerciseNames = "Yoga", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 3.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Pilates", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Stretching", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 2.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Tai Chi", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 3.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Foam Rolling", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 2.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Dynamic Stretching", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 3.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Static Stretching", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 2.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "PNF Stretching", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 2.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Mobility Drills", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 3.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Ballet", ExerciseTypes = "Flexibility", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Moderate" },
                
                // HIIT/Circuit Training
                new ExerciseType { ExerciseNames = "Burpees", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 12.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Mountain Climbers", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "High Knees", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Jumping Jacks", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Box Jumps", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 11.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Plank Jacks", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Squat Jumps", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 10.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Tuck Jumps", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Bear Crawls", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 9.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Crab Walks", ExerciseTypes = "HIIT", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "Medium" },
                
                // Core Exercises
                new ExerciseType { ExerciseNames = "Plank", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Crunches", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 3.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Sit-ups", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Russian Twists", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Leg Raises", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Bicycle Crunches", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Side Plank", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Dead Bug", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 3.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Bird Dog", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 3.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Ab Wheel Rollout", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Hanging Knee Raises", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 5.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Cable Crunches", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Woodchoppers", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Pallof Press", ExerciseTypes = "Core", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Medium" },
                
                // Sports & Recreation
                new ExerciseType { ExerciseNames = "Volleyball", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Golf", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 4.0, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Rock Climbing", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Ice Skating", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Roller Skating", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Surfing", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 6.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Kayaking", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 5.5, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Canoeing", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Horseback Riding", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 4.5, IntensityLevel = "Low" },
                new ExerciseType { ExerciseNames = "Skiing", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Snowboarding", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 7.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Martial Arts", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Fencing", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 6.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Cricket", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 5.0, IntensityLevel = "Medium" },
                new ExerciseType { ExerciseNames = "Rugby", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "American Football", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Lacrosse", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 8.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Field Hockey", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Water Polo", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Squash", ExerciseTypes = "Sports", CaloriesBurnedPerMinute = 12.0, IntensityLevel = "High" },
                
                // Additional Functional Training
                new ExerciseType { ExerciseNames = "Sled Push", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Sled Pull", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Tire Flips", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 12.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Medicine Ball Slams", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Wall Balls", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 8.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Sandbag Training", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 8.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Sledgehammer Swings", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 10.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Turkish Get-ups", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 7.0, IntensityLevel = "Moderate" },
                new ExerciseType { ExerciseNames = "Rope Climbing", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Prowler Push", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 11.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Clean and Jerk", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 9.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Snatch", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 9.5, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Thruster", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 10.0, IntensityLevel = "High" },
                new ExerciseType { ExerciseNames = "Man Makers", ExerciseTypes = "Functional", CaloriesBurnedPerMinute = 11.0, IntensityLevel = "High" }
            };

            context.exercise_types.AddRange(exerciseTypes);
        }

        private static void SeedFoodTypes(AppDbContext context)
        {
            var foodTypes = new List<FoodType>
            {
                // Proteins
                new FoodType { Food_Name = "Chicken Breast", Food_Type = "Protein", Calories_Per_Gram = 1.65, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.31, Fat_Per_Gram = 0.036 },
                new FoodType { Food_Name = "Salmon", Food_Type = "Protein", Calories_Per_Gram = 2.08, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.20, Fat_Per_Gram = 0.13 },
                new FoodType { Food_Name = "Eggs", Food_Type = "Protein", Calories_Per_Gram = 1.55, Carbs_Per_Gram = 0.011, Protein_Per_Gram = 0.13, Fat_Per_Gram = 0.11 },
                new FoodType { Food_Name = "Greek Yogurt", Food_Type = "Protein", Calories_Per_Gram = 0.59, Carbs_Per_Gram = 0.036, Protein_Per_Gram = 0.10, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Tuna", Food_Type = "Protein", Calories_Per_Gram = 1.16, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.26, Fat_Per_Gram = 0.006 },
                new FoodType { Food_Name = "Turkey Breast", Food_Type = "Protein", Calories_Per_Gram = 1.35, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.29, Fat_Per_Gram = 0.017 },
                new FoodType { Food_Name = "Lean Beef", Food_Type = "Protein", Calories_Per_Gram = 2.50, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.26, Fat_Per_Gram = 0.15 },
                new FoodType { Food_Name = "Pork Chop", Food_Type = "Protein", Calories_Per_Gram = 2.31, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.27, Fat_Per_Gram = 0.13 },
                new FoodType { Food_Name = "Cod", Food_Type = "Protein", Calories_Per_Gram = 0.82, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.18, Fat_Per_Gram = 0.007 },
                new FoodType { Food_Name = "Shrimp", Food_Type = "Protein", Calories_Per_Gram = 0.99, Carbs_Per_Gram = 0.009, Protein_Per_Gram = 0.24, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Tilapia", Food_Type = "Protein", Calories_Per_Gram = 0.96, Carbs_Per_Gram = 0, Protein_Per_Gram = 0.20, Fat_Per_Gram = 0.017 },
                new FoodType { Food_Name = "Cottage Cheese", Food_Type = "Protein", Calories_Per_Gram = 0.98, Carbs_Per_Gram = 0.033, Protein_Per_Gram = 0.11, Fat_Per_Gram = 0.043 },
                new FoodType { Food_Name = "Tofu", Food_Type = "Protein", Calories_Per_Gram = 0.76, Carbs_Per_Gram = 0.019, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.048 },
                new FoodType { Food_Name = "Tempeh", Food_Type = "Protein", Calories_Per_Gram = 1.93, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.19, Fat_Per_Gram = 0.11 },
                new FoodType { Food_Name = "Seitan", Food_Type = "Protein", Calories_Per_Gram = 3.70, Carbs_Per_Gram = 0.14, Protein_Per_Gram = 0.75, Fat_Per_Gram = 0.02 },
                
                // Carbohydrates
                new FoodType { Food_Name = "Brown Rice", Food_Type = "Carbs", Calories_Per_Gram = 1.12, Carbs_Per_Gram = 0.23, Protein_Per_Gram = 0.026, Fat_Per_Gram = 0.009 },
                new FoodType { Food_Name = "Oatmeal", Food_Type = "Carbs", Calories_Per_Gram = 3.89, Carbs_Per_Gram = 0.66, Protein_Per_Gram = 0.17, Fat_Per_Gram = 0.07 },
                new FoodType { Food_Name = "Sweet Potato", Food_Type = "Carbs", Calories_Per_Gram = 0.86, Carbs_Per_Gram = 0.20, Protein_Per_Gram = 0.016, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Quinoa", Food_Type = "Carbs", Calories_Per_Gram = 1.20, Carbs_Per_Gram = 0.21, Protein_Per_Gram = 0.043, Fat_Per_Gram = 0.019 },
                new FoodType { Food_Name = "Whole Wheat Bread", Food_Type = "Carbs", Calories_Per_Gram = 2.47, Carbs_Per_Gram = 0.41, Protein_Per_Gram = 0.13, Fat_Per_Gram = 0.034 },
                new FoodType { Food_Name = "White Rice", Food_Type = "Carbs", Calories_Per_Gram = 1.30, Carbs_Per_Gram = 0.28, Protein_Per_Gram = 0.027, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Pasta", Food_Type = "Carbs", Calories_Per_Gram = 1.31, Carbs_Per_Gram = 0.25, Protein_Per_Gram = 0.05, Fat_Per_Gram = 0.009 },
                new FoodType { Food_Name = "Potatoes", Food_Type = "Carbs", Calories_Per_Gram = 0.77, Carbs_Per_Gram = 0.17, Protein_Per_Gram = 0.020, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Couscous", Food_Type = "Carbs", Calories_Per_Gram = 1.12, Carbs_Per_Gram = 0.23, Protein_Per_Gram = 0.038, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Barley", Food_Type = "Carbs", Calories_Per_Gram = 1.23, Carbs_Per_Gram = 0.28, Protein_Per_Gram = 0.028, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Buckwheat", Food_Type = "Carbs", Calories_Per_Gram = 3.43, Carbs_Per_Gram = 0.72, Protein_Per_Gram = 0.13, Fat_Per_Gram = 0.034 },
                new FoodType { Food_Name = "Corn", Food_Type = "Carbs", Calories_Per_Gram = 0.86, Carbs_Per_Gram = 0.19, Protein_Per_Gram = 0.032, Fat_Per_Gram = 0.012 },
                new FoodType { Food_Name = "Pita Bread", Food_Type = "Carbs", Calories_Per_Gram = 2.75, Carbs_Per_Gram = 0.55, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.012 },
                new FoodType { Food_Name = "Tortilla", Food_Type = "Carbs", Calories_Per_Gram = 3.18, Carbs_Per_Gram = 0.49, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.10 },
                new FoodType { Food_Name = "Rice Cakes", Food_Type = "Carbs", Calories_Per_Gram = 3.92, Carbs_Per_Gram = 0.82, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.03 },
                
                // Vegetables
                new FoodType { Food_Name = "Broccoli", Food_Type = "Vegetable", Calories_Per_Gram = 0.34, Carbs_Per_Gram = 0.07, Protein_Per_Gram = 0.028, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Spinach", Food_Type = "Vegetable", Calories_Per_Gram = 0.23, Carbs_Per_Gram = 0.036, Protein_Per_Gram = 0.029, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Kale", Food_Type = "Vegetable", Calories_Per_Gram = 0.49, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.043, Fat_Per_Gram = 0.009 },
                new FoodType { Food_Name = "Carrots", Food_Type = "Vegetable", Calories_Per_Gram = 0.41, Carbs_Per_Gram = 0.10, Protein_Per_Gram = 0.009, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Bell Peppers", Food_Type = "Vegetable", Calories_Per_Gram = 0.31, Carbs_Per_Gram = 0.06, Protein_Per_Gram = 0.01, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Cauliflower", Food_Type = "Vegetable", Calories_Per_Gram = 0.25, Carbs_Per_Gram = 0.05, Protein_Per_Gram = 0.019, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Zucchini", Food_Type = "Vegetable", Calories_Per_Gram = 0.17, Carbs_Per_Gram = 0.033, Protein_Per_Gram = 0.012, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Asparagus", Food_Type = "Vegetable", Calories_Per_Gram = 0.20, Carbs_Per_Gram = 0.039, Protein_Per_Gram = 0.022, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Green Beans", Food_Type = "Vegetable", Calories_Per_Gram = 0.31, Carbs_Per_Gram = 0.07, Protein_Per_Gram = 0.018, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Brussels Sprouts", Food_Type = "Vegetable", Calories_Per_Gram = 0.43, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.034, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Cucumber", Food_Type = "Vegetable", Calories_Per_Gram = 0.15, Carbs_Per_Gram = 0.036, Protein_Per_Gram = 0.007, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Tomatoes", Food_Type = "Vegetable", Calories_Per_Gram = 0.18, Carbs_Per_Gram = 0.039, Protein_Per_Gram = 0.009, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Lettuce", Food_Type = "Vegetable", Calories_Per_Gram = 0.15, Carbs_Per_Gram = 0.029, Protein_Per_Gram = 0.014, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Mushrooms", Food_Type = "Vegetable", Calories_Per_Gram = 0.22, Carbs_Per_Gram = 0.033, Protein_Per_Gram = 0.031, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Onions", Food_Type = "Vegetable", Calories_Per_Gram = 0.40, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.011, Fat_Per_Gram = 0.001 },
                
                // Fruits
                new FoodType { Food_Name = "Banana", Food_Type = "Fruit", Calories_Per_Gram = 0.89, Carbs_Per_Gram = 0.23, Protein_Per_Gram = 0.011, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Apple", Food_Type = "Fruit", Calories_Per_Gram = 0.52, Carbs_Per_Gram = 0.14, Protein_Per_Gram = 0.003, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Orange", Food_Type = "Fruit", Calories_Per_Gram = 0.47, Carbs_Per_Gram = 0.12, Protein_Per_Gram = 0.009, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Strawberries", Food_Type = "Fruit", Calories_Per_Gram = 0.32, Carbs_Per_Gram = 0.077, Protein_Per_Gram = 0.007, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Blueberries", Food_Type = "Fruit", Calories_Per_Gram = 0.57, Carbs_Per_Gram = 0.14, Protein_Per_Gram = 0.007, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Grapes", Food_Type = "Fruit", Calories_Per_Gram = 0.69, Carbs_Per_Gram = 0.18, Protein_Per_Gram = 0.007, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Watermelon", Food_Type = "Fruit", Calories_Per_Gram = 0.30, Carbs_Per_Gram = 0.076, Protein_Per_Gram = 0.006, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Mango", Food_Type = "Fruit", Calories_Per_Gram = 0.60, Carbs_Per_Gram = 0.15, Protein_Per_Gram = 0.008, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Pineapple", Food_Type = "Fruit", Calories_Per_Gram = 0.50, Carbs_Per_Gram = 0.13, Protein_Per_Gram = 0.005, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Pear", Food_Type = "Fruit", Calories_Per_Gram = 0.57, Carbs_Per_Gram = 0.15, Protein_Per_Gram = 0.004, Fat_Per_Gram = 0.001 },
                new FoodType { Food_Name = "Peach", Food_Type = "Fruit", Calories_Per_Gram = 0.39, Carbs_Per_Gram = 0.10, Protein_Per_Gram = 0.009, Fat_Per_Gram = 0.003 },
                new FoodType { Food_Name = "Cherries", Food_Type = "Fruit", Calories_Per_Gram = 0.63, Carbs_Per_Gram = 0.16, Protein_Per_Gram = 0.011, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Kiwi", Food_Type = "Fruit", Calories_Per_Gram = 0.61, Carbs_Per_Gram = 0.15, Protein_Per_Gram = 0.011, Fat_Per_Gram = 0.005 },
                new FoodType { Food_Name = "Raspberries", Food_Type = "Fruit", Calories_Per_Gram = 0.52, Carbs_Per_Gram = 0.12, Protein_Per_Gram = 0.012, Fat_Per_Gram = 0.007 },
                new FoodType { Food_Name = "Blackberries", Food_Type = "Fruit", Calories_Per_Gram = 0.43, Carbs_Per_Gram = 0.10, Protein_Per_Gram = 0.014, Fat_Per_Gram = 0.005 },
                
                // Healthy Fats
                new FoodType { Food_Name = "Almonds", Food_Type = "Fats", Calories_Per_Gram = 5.79, Carbs_Per_Gram = 0.22, Protein_Per_Gram = 0.21, Fat_Per_Gram = 0.50 },
                new FoodType { Food_Name = "Walnuts", Food_Type = "Fats", Calories_Per_Gram = 6.54, Carbs_Per_Gram = 0.14, Protein_Per_Gram = 0.15, Fat_Per_Gram = 0.65 },
                new FoodType { Food_Name = "Cashews", Food_Type = "Fats", Calories_Per_Gram = 5.53, Carbs_Per_Gram = 0.30, Protein_Per_Gram = 0.18, Fat_Per_Gram = 0.44 },
                new FoodType { Food_Name = "Peanuts", Food_Type = "Fats", Calories_Per_Gram = 5.67, Carbs_Per_Gram = 0.16, Protein_Per_Gram = 0.26, Fat_Per_Gram = 0.49 },
                new FoodType { Food_Name = "Pistachios", Food_Type = "Fats", Calories_Per_Gram = 5.60, Carbs_Per_Gram = 0.28, Protein_Per_Gram = 0.20, Fat_Per_Gram = 0.45 },
                new FoodType { Food_Name = "Avocado", Food_Type = "Fats", Calories_Per_Gram = 1.60, Carbs_Per_Gram = 0.085, Protein_Per_Gram = 0.020, Fat_Per_Gram = 0.15 },
                new FoodType { Food_Name = "Olive Oil", Food_Type = "Fats", Calories_Per_Gram = 8.84, Carbs_Per_Gram = 0, Protein_Per_Gram = 0, Fat_Per_Gram = 1.0 },
                new FoodType { Food_Name = "Coconut Oil", Food_Type = "Fats", Calories_Per_Gram = 8.62, Carbs_Per_Gram = 0, Protein_Per_Gram = 0, Fat_Per_Gram = 0.99 },
                new FoodType { Food_Name = "Peanut Butter", Food_Type = "Fats", Calories_Per_Gram = 5.88, Carbs_Per_Gram = 0.20, Protein_Per_Gram = 0.25, Fat_Per_Gram = 0.50 },
                new FoodType { Food_Name = "Almond Butter", Food_Type = "Fats", Calories_Per_Gram = 6.14, Carbs_Per_Gram = 0.19, Protein_Per_Gram = 0.21, Fat_Per_Gram = 0.56 },
                new FoodType { Food_Name = "Chia Seeds", Food_Type = "Fats", Calories_Per_Gram = 4.86, Carbs_Per_Gram = 0.42, Protein_Per_Gram = 0.17, Fat_Per_Gram = 0.31 },
                new FoodType { Food_Name = "Flax Seeds", Food_Type = "Fats", Calories_Per_Gram = 5.34, Carbs_Per_Gram = 0.29, Protein_Per_Gram = 0.18, Fat_Per_Gram = 0.42 },
                new FoodType { Food_Name = "Sunflower Seeds", Food_Type = "Fats", Calories_Per_Gram = 5.84, Carbs_Per_Gram = 0.20, Protein_Per_Gram = 0.21, Fat_Per_Gram = 0.52 },
                new FoodType { Food_Name = "Pumpkin Seeds", Food_Type = "Fats", Calories_Per_Gram = 5.59, Carbs_Per_Gram = 0.11, Protein_Per_Gram = 0.30, Fat_Per_Gram = 0.49 },
                new FoodType { Food_Name = "Hemp Seeds", Food_Type = "Fats", Calories_Per_Gram = 5.53, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.32, Fat_Per_Gram = 0.49 },
                
                // Legumes
                new FoodType { Food_Name = "Black Beans", Food_Type = "Legume", Calories_Per_Gram = 1.32, Carbs_Per_Gram = 0.24, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.005 },
                new FoodType { Food_Name = "Kidney Beans", Food_Type = "Legume", Calories_Per_Gram = 1.27, Carbs_Per_Gram = 0.23, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.005 },
                new FoodType { Food_Name = "Chickpeas", Food_Type = "Legume", Calories_Per_Gram = 1.64, Carbs_Per_Gram = 0.27, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.026 },
                new FoodType { Food_Name = "Lentils", Food_Type = "Legume", Calories_Per_Gram = 1.16, Carbs_Per_Gram = 0.20, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Pinto Beans", Food_Type = "Legume", Calories_Per_Gram = 1.43, Carbs_Per_Gram = 0.26, Protein_Per_Gram = 0.09, Fat_Per_Gram = 0.007 },
                new FoodType { Food_Name = "Navy Beans", Food_Type = "Legume", Calories_Per_Gram = 1.40, Carbs_Per_Gram = 0.26, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.006 },
                new FoodType { Food_Name = "Lima Beans", Food_Type = "Legume", Calories_Per_Gram = 1.15, Carbs_Per_Gram = 0.21, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Split Peas", Food_Type = "Legume", Calories_Per_Gram = 1.18, Carbs_Per_Gram = 0.21, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.004 },
                new FoodType { Food_Name = "Soybeans", Food_Type = "Legume", Calories_Per_Gram = 1.73, Carbs_Per_Gram = 0.11, Protein_Per_Gram = 0.17, Fat_Per_Gram = 0.09 },
                new FoodType { Food_Name = "Edamame", Food_Type = "Legume", Calories_Per_Gram = 1.22, Carbs_Per_Gram = 0.09, Protein_Per_Gram = 0.11, Fat_Per_Gram = 0.05 },
                
                // Dairy & Alternatives
                new FoodType { Food_Name = "Milk", Food_Type = "Dairy", Calories_Per_Gram = 0.42, Carbs_Per_Gram = 0.05, Protein_Per_Gram = 0.034, Fat_Per_Gram = 0.01 },
                new FoodType { Food_Name = "Cheddar Cheese", Food_Type = "Dairy", Calories_Per_Gram = 4.03, Carbs_Per_Gram = 0.013, Protein_Per_Gram = 0.25, Fat_Per_Gram = 0.33 },
                new FoodType { Food_Name = "Mozzarella", Food_Type = "Dairy", Calories_Per_Gram = 3.00, Carbs_Per_Gram = 0.024, Protein_Per_Gram = 0.22, Fat_Per_Gram = 0.22 },
                new FoodType { Food_Name = "Feta Cheese", Food_Type = "Dairy", Calories_Per_Gram = 2.64, Carbs_Per_Gram = 0.044, Protein_Per_Gram = 0.14, Fat_Per_Gram = 0.21 },
                new FoodType { Food_Name = "Parmesan", Food_Type = "Dairy", Calories_Per_Gram = 4.31, Carbs_Per_Gram = 0.036, Protein_Per_Gram = 0.38, Fat_Per_Gram = 0.29 },
                new FoodType { Food_Name = "Yogurt", Food_Type = "Dairy", Calories_Per_Gram = 0.61, Carbs_Per_Gram = 0.047, Protein_Per_Gram = 0.035, Fat_Per_Gram = 0.032 },
                new FoodType { Food_Name = "Almond Milk", Food_Type = "Dairy Alternative", Calories_Per_Gram = 0.17, Carbs_Per_Gram = 0.007, Protein_Per_Gram = 0.004, Fat_Per_Gram = 0.011 },
                new FoodType { Food_Name = "Soy Milk", Food_Type = "Dairy Alternative", Calories_Per_Gram = 0.33, Carbs_Per_Gram = 0.012, Protein_Per_Gram = 0.028, Fat_Per_Gram = 0.016 },
                new FoodType { Food_Name = "Oat Milk", Food_Type = "Dairy Alternative", Calories_Per_Gram = 0.47, Carbs_Per_Gram = 0.083, Protein_Per_Gram = 0.010, Fat_Per_Gram = 0.015 },
                new FoodType { Food_Name = "Coconut Milk", Food_Type = "Dairy Alternative", Calories_Per_Gram = 2.30, Carbs_Per_Gram = 0.06, Protein_Per_Gram = 0.023, Fat_Per_Gram = 0.24 },
                
                // Snacks & Others
                new FoodType { Food_Name = "Dark Chocolate", Food_Type = "Snack", Calories_Per_Gram = 5.99, Carbs_Per_Gram = 0.46, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.43 },
                new FoodType { Food_Name = "Honey", Food_Type = "Sweetener", Calories_Per_Gram = 3.04, Carbs_Per_Gram = 0.82, Protein_Per_Gram = 0.003, Fat_Per_Gram = 0 },
                new FoodType { Food_Name = "Maple Syrup", Food_Type = "Sweetener", Calories_Per_Gram = 2.60, Carbs_Per_Gram = 0.67, Protein_Per_Gram = 0.001, Fat_Per_Gram = 0.002 },
                new FoodType { Food_Name = "Hummus", Food_Type = "Snack", Calories_Per_Gram = 1.66, Carbs_Per_Gram = 0.14, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.10 },
                new FoodType { Food_Name = "Granola", Food_Type = "Snack", Calories_Per_Gram = 4.71, Carbs_Per_Gram = 0.65, Protein_Per_Gram = 0.10, Fat_Per_Gram = 0.20 },
                new FoodType { Food_Name = "Protein Powder", Food_Type = "Supplement", Calories_Per_Gram = 4.00, Carbs_Per_Gram = 0.05, Protein_Per_Gram = 0.80, Fat_Per_Gram = 0.05 },
                new FoodType { Food_Name = "Beef Jerky", Food_Type = "Snack", Calories_Per_Gram = 4.10, Carbs_Per_Gram = 0.11, Protein_Per_Gram = 0.33, Fat_Per_Gram = 0.25 },
                new FoodType { Food_Name = "Rice Crackers", Food_Type = "Snack", Calories_Per_Gram = 3.80, Carbs_Per_Gram = 0.81, Protein_Per_Gram = 0.08, Fat_Per_Gram = 0.02 },
                new FoodType { Food_Name = "Popcorn", Food_Type = "Snack", Calories_Per_Gram = 3.87, Carbs_Per_Gram = 0.78, Protein_Per_Gram = 0.13, Fat_Per_Gram = 0.04 },
                new FoodType { Food_Name = "Trail Mix", Food_Type = "Snack", Calories_Per_Gram = 4.62, Carbs_Per_Gram = 0.44, Protein_Per_Gram = 0.14, Fat_Per_Gram = 0.30 }
            };

            context.Food_items.AddRange(foodTypes);
        }
    }
}
