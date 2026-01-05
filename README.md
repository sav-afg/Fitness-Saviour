# Fitness Saviour

A full-stack fitness web application built with C#, Blazor, and SQL Server. It provides personalized diet and exercise recommendations, allows users to log calories and macros, and visualizes progress with charts.

### Quick facts
- Framework: .NET 10 (Blazor)
- UI: Blazor (Razor components), HTML, CSS
- Backend: C#, Entity Framework Core
- Database: SQL Server

## Features
- Daily calorie accumulator with user targets and history (`FoodEntry`, `ExerciseEntry`).
- Personalized diet recommendations via questionnaire and scoring algorithm.
- Exercise guidance (cardio, resistance, flexibility) with safe, general recommendations.
- Data visualization for macro and calorie breakdowns (charts in `wwwroot` + components).
- Persistent storage using EF Core and SQL Server.

## How it works (summary)
- Users complete a questionnaire covering goals, preferences, and constraints.
- The recommendation pipeline:
  1. Filter diet options by hard constraints (allergies, preferences).
  2. Score remaining diets against user responses:
     - Weight for goals (e.g., weight loss, muscle gain).
     - Preferences (macros, meal frequency).
     - Practical constraints (budget, time).
  3. Rank diets by score and present top results with breakdown charts.
- Calorie accumulator:
  - Each `FoodEntry` adds calories/macros to the day's total.
  - Each `ExerciseEntry` subtracts estimated calories burned.
  - Daily target stored on the user profile; progress visualized in UI.

## Data model
<ul>
  <li>
    <strong>User</strong> – Represents an authenticated application user and stores preferences, accessibility settings, and aggregated progress metrics.
    <ul>
      <li>UserId</li>
      <li>Username</li>
      <li>PasswordHash</li>
      <li>EncryptedEmail</li>
      <li>EncryptedPhoneNumber</li>
      <li>Role</li>
      <li>CreatedAt</li>
      <li>LoginStreak</li>
      <li>
        <strong>Nutrition Tracking</strong>
        <ul>
          <li>DailyCalories</li>
          <li>DailyCarbs</li>
          <li>DailyProtein</li>
          <li>DailyFat</li>
          <li>WeeklyCalories</li>
          <li>WeeklyCarbs</li>
          <li>WeeklyProtein</li>
          <li>WeeklyFat</li>
          <li>TotalCalories</li>
        </ul>
      </li>
      <li>
        <strong>Accessibility &amp; UI Preferences</strong>
        <ul>
          <li>HighContrastMode</li>
          <li>DyslexiaFont</li>
          <li>ReducedAnimations</li>
          <li>LargerTextSize</li>
          <li>MinimalInterface</li>
        </ul>
      </li>
      <li>
        <strong>Engagement &amp; Tracking Preferences</strong>
        <ul>
          <li>TrackingPreferences</li>
          <li>VisualRewards</li>
          <li>ProgressData</li>
        </ul>
      </li>
    </ul>
  </li>

  <li>
    <strong>FoodItem</strong> – Represents a single food item with macronutrient density values.
    <ul>
      <li>FoodId</li>
      <li>FoodName</li>
      <li>FoodType</li>
      <li>CaloriesPerGram</li>
      <li>CarbsPerGram</li>
      <li>ProteinPerGram</li>
      <li>FatPerGram</li>
    </ul>
  </li>

  <li>
    <strong>UserFoodItem</strong> – Join entity linking users to food items they have consumed or tracked.
    <ul>
      <li>UserFoodItemId</li>
      <li>UserId</li>
      <li>FoodId</li>
    </ul>
  </li>

  <li>
    <strong>ExerciseType</strong> – Represents a generic exercise category (e.g. cardio, resistance, flexibility).
    <ul>
      <li>ExerciseId</li>
      <li>ExerciseName</li>
      <li>ExerciseType</li>
      <li>CaloriesBurntPerMinute</li>
      <li>Intensity</li>
    </ul>
  </li>

  <li>
    <strong>HypertrophyExercise</strong> – Represents resistance-training exercises with body-part targeting.
    <ul>
      <li>HExerciseId</li>
      <li>BodyPart</li>
      <li>CaloriesBurntPerRep</li>
      <li>ExerciseId (FK → ExerciseType)</li>
    </ul>
  </li>

  <li>
    <strong>Graph</strong> – Defines visualisation metadata used to display progress and recommendations.
    <ul>
      <li>GraphId</li>
      <li>GraphCategory</li>
      <li>GraphType</li>
    </ul>
  </li>

  <li>
    <strong>FoodItemGraph</strong> – Associates food items with visual graph representations.
    <ul>
      <li>FoodItemGraphId</li>
      <li>FoodId</li>
      <li>GraphId</li>
    </ul>
  </li>

  <li>
    <strong>ExerciseGraph</strong> – Associates exercises with visual graph representations.
    <ul>
      <li>ExerciseGraphId</li>
      <li>ExerciseId</li>
      <li>GraphId</li>
    </ul>
  </li>

  <li>
    <strong>UserGraph</strong> – Tracks which graphs are relevant or enabled for a given user.
    <ul>
      <li>UserGraphId</li>
      <li>UserId</li>
      <li>GraphId</li>
    </ul>
  </li>
</ul>

Files to look for:
- `Data/ApplicationDbContext.cs`
- `Models/*` (e.g., `Diet.cs`, `FoodEntry.cs`, `ExerciseEntry.cs`, `User.cs`)
- `Services/*` (recommendation engine, logging, data services)
- Blazor pages: `Pages/Questionnaire.razor`, `Pages/Dashboard.razor`, `Pages/Recommendations.razor`
- Reusable components: `Components/Chart.razor`, `Components/EntryForm.razor`

## Running locally

Prerequisites:
- .NET 10 SDK
- SQL Server (local or remote)
- Optional: `dotnet-ef` tools (for migrations)

Setup:
1. Copy `appsettings.example.json` to `appsettings.Development.json` and set the connection string for SQL Server:
   - e.g. `"ConnectionStrings:DefaultConnection": "Server=.;Database=FitnessSaviourDb;Trusted_Connection=True;TrustServerCertificate=True;"`
2. Apply EF Core migrations:
   - `dotnet tool install --global dotnet-ef` (if not installed)
   - `dotnet ef database update --context ApplicationDbContext`
   - Or run migrations on startup if configured.
3. Run the app:
   - `dotnet run --project src/FitnessSaviour` (adjust path to project)
   - Or press F5 / run from Visual Studio.

Database migration quick commands:
- Add migration: `dotnet ef migrations add InitialCreate --project src/FitnessSaviour --startup-project src/FitnessSaviour`
- Update database: `dotnet ef database update --project src/FitnessSaviour --startup-project src/FitnessSaviour`

Running tests:
- `dotnet test`

## Development notes
- Blazor layout:
  - UI pages live under `Pages/`
  - Shared layout and components under `Shared/` and `Components/`
  - Static assets in `wwwroot/`
- Dependency Injection:
  - Register services (recommendation engine, repositories) in `Program.cs`.

## Privacy & Safety
- Does not provide prescriptive medical or clinical advice.
- Provides general, safe exercise suggestions and prompts to consult healthcare professionals when necessary.


## Contact / More info
- Project developed as coursework for A-level Computer Science.
- For architecture diagrams and development screenshots, refer to the "Application Development Screenshots" document bundled with the repo.
