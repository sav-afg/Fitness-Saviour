# In-Memory Database - Seeded User Accounts

Your application now uses an **in-memory database** that is automatically seeded with sample data each time the application starts.

## ?? Test User Accounts

Three user accounts are available for testing, each with unique characteristics and 30+ days of historical data:

### 1. **John Fitness** (Weight Loss Journey)
- **Username:** `john_fitness`
- **Password:** `Password123!`
- **Email:** john@example.com
- **Goal:** Fat Loss
- **Stats:**
  - Body Weight: 85 kg (trending down -0.15 kg/week)
  - Maintenance Calories: 2500 kcal/day
  - Daily Intake: ~2650 kcal (slight surplus due to variance)
  - Macros: 300g carbs, 150g protein, 80g fat
  - Daily Exercise: 450 kcal burned (30min cardio, 45min strength, 15min flexibility)
  - Login Streak: 15 days

### 2. **Sarah Health** (Maintenance)
- **Username:** `sarah_health`
- **Password:** `Password123!`
- **Email:** sarah@example.com
- **Goal:** Health & Wellness
- **Stats:**
  - Body Weight: 68 kg (maintaining, +0.05 kg/week)
  - Maintenance Calories: 2000 kcal/day
  - Daily Intake: ~1850 kcal (slight deficit)
  - Macros: 200g carbs, 120g protein, 65g fat
  - Daily Exercise: 350 kcal burned (40min cardio, 30min strength, 20min flexibility)
  - Login Streak: 30 days
  - UI Preferences: High Contrast Mode enabled

### 3. **Mike Athlete** (Muscle Gain)
- **Username:** `mike_athlete`
- **Password:** `Password123!`
- **Email:** mike@example.com
- **Goal:** Muscle Gain / Bulking
- **Stats:**
  - Body Weight: 92 kg (trending up +0.2 kg/week)
  - Maintenance Calories: 3000 kcal/day
  - Daily Intake: ~3200 kcal (surplus for gains)
  - Macros: 400g carbs, 180g protein, 90g fat
  - Daily Exercise: 600 kcal burned (20min cardio, 60min strength, 10min flexibility)
  - Login Streak: 45 days
  - UI Preferences: Dyslexia-friendly font, larger text size

## ?? Available Data

Each user account includes:

### Weight Logs
- **Duration:** Past 56 days (8 weeks)
- **Frequency:** Every 2-3 days (realistic pattern)
- **Trend:** Each user has a unique weight trend (loss/maintenance/gain)
- **Variance:** Natural daily fluctuations (±0.5 kg)

### Calorie Logs
- **Duration:** Past 30 days
- **Entries per Day:** 2-4 meal logs (breakfast, lunch, dinner, snacks)
- **Data Included:**
  - Calories consumed per meal
  - Calories burned through exercise (logged once per day)
  - Net calories
  - Macro breakdown (carbs, protein, fat in grams)
  - Calorie contribution from each macro

### Exercise Types
10 pre-loaded exercise types across categories:
- **Cardio:** Running, Cycling, Swimming, Walking
- **Strength:** Bench Press, Squats, Deadlifts
- **Flexibility:** Yoga, Pilates, Stretching

### Food Types
10 common foods with accurate nutritional data:
- **Proteins:** Chicken Breast, Salmon, Eggs, Greek Yogurt
- **Carbs:** Brown Rice, Oatmeal, Sweet Potato
- **Vegetables:** Broccoli
- **Fruits:** Banana
- **Fats:** Almonds

## ?? Graphs That Will Display Data

All graphs should now render with realistic data:

1. ? **Bodyweight over Time** - 8 weeks of weight tracking
2. ? **Weight Change per Week** - Weekly weight trends
3. ? **Daily Calorie Intake vs Target** - 30 days of intake vs maintenance calories
4. ? **Daily Calorie Surplus/Deficit** - Color-coded surplus (green) vs deficit (red)
5. ? **Average Macro Distribution** - Pie chart of carbs/protein/fat ratio
6. ? **Daily Macro Intake** - Average macro consumption over 6 days
7. ? **Calories Burnt Through Exercise** - Exercise calorie burn over 6 days
8. ? **Exercise Type Frequency** - Distribution of exercise types

## ?? Data Persistence

**Important:** The in-memory database is **reset every time** the application restarts. All data reverts to the seeded state. This is perfect for:
- ? Testing without worrying about data corruption
- ? Demonstrating features with realistic data
- ? Development without needing SQL Server setup

## ?? Troubleshooting

### Login Issues

If the seeded accounts aren't working:

1. **Check the console output** when the app starts - you should see:
   ```
   ?? Starting database seeding...
   ? Database seeding completed successfully!
      ?? Users: 3
      ??  Weight Logs: [number]
      ???  Calorie Logs: [number]
   ```

2. **Verify you're using the correct credentials:**
   - Username: `john_fitness` (lowercase, with underscore)
   - Password: `Password123!` (exact case, with exclamation mark)

3. **Check for error messages** on the login page after attempting to log in

4. **Clear browser cache** - Sometimes old session data can interfere

5. **Restart the application** - The in-memory database is created fresh on each startup

### Database Connection Issues

If you're still seeing SQL Server connection errors:

1. Remove or comment out the `DefaultConnection` from `appsettings.json`
2. Verify `Program.cs` contains:
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options => 
       options.UseInMemoryDatabase("FitnessTrackerDB"));
   ```
3. Ensure `AuthService.cs` uses `AppDbContext`, not `SqlConnection`

## ?? Testing Recommendations

1. **Log in as different users** to see varying data patterns
2. **Compare graphs** between users to see weight loss vs gain trends
3. **Test UI preferences** with Sarah (high contrast) and Mike (dyslexia-friendly font)
4. **Add new data** and see it appear in graphs (will reset on app restart)

## ??? Switching Back to SQL Server

If you need to switch back to SQL Server later:

1. Update `Program.cs`:
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options => 
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

2. Comment out or remove the seeding code:
   ```csharp
   // using (var scope = app.Services.CreateScope())
   // {
   //     var services = scope.ServiceProvider;
   //     var context = services.GetRequiredService<AppDbContext>();
   //     DatabaseSeeder.SeedDatabase(context);
   // }
   ```

3. Run migrations to create the database schema
