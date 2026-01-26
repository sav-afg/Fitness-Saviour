# Fitness Saviour

A comprehensive full-stack fitness web application built with C#, Blazor, and SQL Server. It provides personalized nutrition and training recommendations, real-time progress tracking, customizable workout plans, and detailed analytics to help users achieve their fitness goals.

### Quick facts
- Framework: .NET 10 (Blazor Server with InteractiveServer render mode)
- UI: Blazor (Razor components), Bootstrap 5, BlazorBootstrap Charts
- Backend: C#, Entity Framework Core
- Database: SQL Server
- Architecture: Service-based with dependency injection

## Features

### Nutrition & Diet Management
- **Maintenance Calorie Calculator**: Science-based calculator using sex, height, weight, age, and activity level to determine daily calorie needs
- **Food Logging**: Track daily food intake with automatic calorie and macro calculations
- **Diet Recommendations**: Personalized diet suggestions via intelligent questionnaire and scoring algorithm that considers:
  - User goals (weight loss, muscle gain, maintenance)
  - Dietary preferences and restrictions
  - Allergies and food intolerances
  - Budget and time constraints
  - Meal frequency preferences
- **Macro Tracking**: Monitor protein, carbohydrates, and fat intake with visual breakdowns

### Exercise & Training
- **Exercise Logging**: Track workouts with automatic calorie burn estimation
- **Exercise Questionnaire**: Get personalized exercise recommendations for cardio, resistance, and flexibility training
- **Hypertrophy/Workout Split Generator**: Create customized resistance training splits based on:
  - Training experience level (beginner, intermediate, advanced)
  - Available training days per week (1-7 days)
  - Target muscle groups (upper, lower, or full body)
  - Personal preference (enjoyment vs. optimization)
- **Exercise Type Tracking**: Log different exercise modalities (cardio, resistance, flexibility)
- **Advanced Metrics**: Optional tracking of RPE (Rate of Perceived Exertion) and RIR (Reps in Reserve)

### Progress Tracking & Analytics
- **Interactive Graphs & Charts**: Multiple visualization types including:
  - Bodyweight progress over time
  - Weight change per week
  - Daily calorie intake vs. target
  - Daily calorie surplus/deficit tracking
  - Average macro distribution (pie charts)
  - Daily macro intake trends
  - Calories burnt through exercise
  - Exercise type frequency analysis
- **Weight Logging**: Track bodyweight changes over time
- **Calorie Logs**: Historical tracking of calories consumed, burned, and net balance
- **Daily & Weekly Progress**: View aggregated nutrition and training data
- **Login Streak Tracking**: Monitor consistency and engagement

### Personalization & Accessibility
- **UI Personalization**: Extensive customization options including:
  - Primary fitness goal selection (fat loss, muscle gain, endurance, general health)
  - Experience level configuration
  - Custom tracking preferences (calories, macros, exercise, weight trends)
  - Motivation preferences (gamification, progress data, minimal interface)
- **Accessibility Features**:
  - High contrast mode
  - Dyslexia-friendly font option
  - Reduced animations mode
  - Larger text size option
  - Minimal interface for focused experience
- **Adaptive UI**: Dynamic motivational quotes based on user goals
- **Advanced Metrics Toggle**: Show/hide advanced training metrics based on experience level

### Account Management
- **User Authentication**: Secure account creation and login system
- **Password Security**: Encrypted password storage with hashing
- **Encrypted Data**: Email and phone number encryption for privacy
- **User Profiles**: Personalized settings and preferences storage
- **Session Management**: Secure session tracking and user state management
- **Account Details**: View and manage personal information

### Legal & Privacy
- **Privacy Policy**: Comprehensive data handling transparency
- **Terms of Service**: Clear usage guidelines and responsibilities
- **Data Protection**: Encrypted sensitive information storage

### User Experience
- **Responsive Design**: Mobile-friendly interface
- **Real-time Updates**: Interactive components with instant feedback
- **Reconnection Handling**: Automatic reconnect modal for lost connections
- **Error Handling**: User-friendly error messages and validation
- **Persistent Storage**: All data saved to SQL Server for long-term tracking

## How it works

### Nutrition Recommendation Pipeline
1. **User Input**: Complete a comprehensive questionnaire covering goals, dietary preferences, allergies, and practical constraints
2. **Filtering**: Remove diet options that conflict with hard constraints (allergies, dietary restrictions)
3. **Scoring Algorithm**:
   - Weight scores based on user goals (weight loss, muscle gain, maintenance)
   - Factor in macro preferences and meal frequency
   - Consider practical constraints (budget, cooking time, meal prep ability)
4. **Results**: Rank diets by compatibility score and present top recommendations with visual macro breakdowns

### Workout Split Generation
1. **Assessment**: Evaluate training experience, available training frequency, and target muscle groups
2. **Algorithm**: Generate personalized workout splits considering:
   - Progressive overload principles appropriate to experience level
   - Optimal training frequency per muscle group
   - Recovery time between sessions
   - User preference for enjoyment vs. optimization
3. **Output**: Graded list of recommended workout splits with detailed explanations

### Calorie & Macro Tracking
- **Food Logging**: Each entry adds calories and macronutrients (protein, carbs, fat) to daily totals
- **Exercise Logging**: Each workout subtracts estimated calories burned based on activity type and duration
- **Net Calculation**: Track daily net calories (consumed - burned) vs. target
- **Historical Analysis**: View trends over time with interactive charts

### Progress Visualization
- **Real-time Charts**: Dynamic updates using BlazorBootstrap chart components
- **Multiple Views**: Line charts for trends, bar charts for comparisons, pie charts for distributions
- **Category Selection**: Choose specific metrics to visualize (bodyweight, calories, macros, exercise)
- **Data Aggregation**: Daily, weekly, and total summaries automatically calculated

## Data model

### Core Entities

<ul>
  <li>
    <strong>User</strong> – Central entity representing an authenticated user with comprehensive tracking and preferences
    <ul>
      <li><strong>Identity & Security</strong>
        <ul>
          <li>UserId (Primary Key)</li>
          <li>Username</li>
          <li>PasswordHash (Securely hashed)</li>
          <li>EncryptedEmail</li>
          <li>EncryptedPhoneNumber</li>
          <li>Role (User/Admin)</li>
          <li>CreatedAt</li>
        </ul>
      </li>
      <li><strong>Engagement Tracking</strong>
        <ul>
          <li>LoginStreak – Consecutive days logged in</li>
        </ul>
      </li>
      <li>
        <strong>Nutrition Tracking</strong>
        <ul>
          <li>DailyCalories, DailyCarbs, DailyProtein, DailyFat</li>
          <li>WeeklyCalories, WeeklyCarbs, WeeklyProtein, WeeklyFat</li>
          <li>TotalCalories (Lifetime total)</li>
        </ul>
      </li>
      <li>
        <strong>Accessibility & UI Preferences</strong>
        <ul>
          <li>HighContrastMode</li>
          <li>DyslexiaFont</li>
          <li>ReducedAnimations</li>
          <li>LargerTextSize</li>
          <li>MinimalInterface</li>
        </ul>
      </li>
      <li>
        <strong>Personalization Settings</strong>
        <ul>
          <li>TrackingPreferences (Calories/Macros/Exercise/Weight)</li>
          <li>VisualRewards (Gamification toggle)</li>
          <li>ProgressData (Data-driven motivation toggle)</li>
          <li>PrimaryGoal (FatLoss/MuscleGain/Endurance/GeneralHealth)</li>
          <li>ExperienceLevel (Beginner/Intermediate/Advanced)</li>
        </ul>
      </li>
    </ul>
  </li>

  <li>
    <strong>FoodType</strong> – Represents food items with complete nutritional information
    <ul>
      <li>FoodId (Primary Key)</li>
      <li>FoodName</li>
      <li>FoodType/Category</li>
      <li>CaloriesPerGram</li>
      <li>CarbsPerGram</li>
      <li>ProteinPerGram</li>
      <li>FatPerGram</li>
    </ul>
  </li>

  <li>
    <strong>UserFoodItems</strong> – Junction table linking users to foods they've consumed
    <ul>
      <li>UserFoodItemId (Primary Key)</li>
      <li>UserId (Foreign Key → User)</li>
      <li>FoodId (Foreign Key → FoodType)</li>
      <li>Quantity/Serving size</li>
      <li>Timestamp</li>
    </ul>
  </li>

  <li>
    <strong>ExerciseType</strong> – Defines exercise categories and base metrics
    <ul>
      <li>ExerciseId (Primary Key)</li>
      <li>ExerciseName</li>
      <li>ExerciseType (Cardio/Resistance/Flexibility)</li>
      <li>CaloriesBurntPerMinute</li>
      <li>Intensity (Low/Medium/High)</li>
    </ul>
  </li>

  <li>
    <strong>HypertrophyExercise</strong> – Specialized resistance training exercises
    <ul>
      <li>HExerciseId (Primary Key)</li>
      <li>BodyPart (Chest/Back/Legs/Shoulders/Arms/Core)</li>
      <li>CaloriesBurntPerRep</li>
      <li>ExerciseId (Foreign Key → ExerciseType)</li>
      <li>Recommended sets/reps based on experience level</li>
    </ul>
  </li>

  <li>
    <strong>UserExercises</strong> – Junction table tracking user exercise activity
    <ul>
      <li>UserExerciseId (Primary Key)</li>
      <li>UserId (Foreign Key → User)</li>
      <li>ExerciseId (Foreign Key → ExerciseType)</li>
      <li>Duration/Sets/Reps</li>
      <li>RPE (Rate of Perceived Exertion, optional)</li>
      <li>RIR (Reps in Reserve, optional)</li>
      <li>Timestamp</li>
    </ul>
  </li>

  <li>
    <strong>WeightLogs</strong> – Historical bodyweight tracking
    <ul>
      <li>WeightLogId (Primary Key)</li>
      <li>UserId (Foreign Key → User)</li>
      <li>LogDate</li>
      <li>Weight (in kg or lbs)</li>
    </ul>
  </li>

  <li>
    <strong>CalorieLogs</strong> – Detailed calorie and macro tracking per day
    <ul>
      <li>CalorieLog_Id (Primary Key)</li>
      <li>User_id (Foreign Key → User)</li>
      <li>Log_Date</li>
      <li>Calories_Consumed</li>
      <li>Calories_Burned (from exercise)</li>
      <li>Net_Calories (Consumed - Burned)</li>
      <li>Calories_From_Carbs</li>
      <li>Calories_From_Protein</li>
      <li>Calories_From_Fats</li>
    </ul>
  </li>

  <li>
    <strong>Graph</strong> – Defines visualization metadata and chart configurations
    <ul>
      <li>GraphId (Primary Key)</li>
      <li>GraphCategory (Bodyweight/Calories/Macros/Exercise/Consistency/Hypertrophy)</li>
      <li>GraphType (Line/Bar/Pie)</li>
    </ul>
  </li>

  <li>
    <strong>FoodItemGraph</strong> – Associates food items with specific visualizations
    <ul>
      <li>FoodItemGraphId (Primary Key)</li>
      <li>FoodId (Foreign Key → FoodType)</li>
      <li>GraphId (Foreign Key → Graph)</li>
    </ul>
  </li>

  <li>
    <strong>ExerciseGraph</strong> – Associates exercises with visual representations
    <ul>
      <li>ExerciseGraphId (Primary Key)</li>
      <li>ExerciseId (Foreign Key → ExerciseType)</li>
      <li>GraphId (Foreign Key → Graph)</li>
    </ul>
  </li>

  <li>
    <strong>UserGraph</strong> – Tracks which graphs each user has enabled/viewed
    <ul>
      <li>UserGraphId (Primary Key)</li>
      <li>UserId (Foreign Key → User)</li>
      <li>GraphId (Foreign Key → Graph)</li>
    </ul>
  </li>
</ul>

### Relationships
- **One-to-Many**: User → WeightLogs, CalorieLogs, UserFoodItems, UserExercises
- **Many-to-Many**: User ↔ FoodType (via UserFoodItems), User ↔ ExerciseType (via UserExercises)
- **Inheritance**: HypertrophyExercise extends ExerciseType for resistance training specifics

## Project structure

### Core Directories
- **`Components/`** – Blazor components and pages
  - **`Pages/`** – Main application pages
    - `Home.razor` – Dashboard with quick access to all features
    - `CreateAccount.razor` – User registration
    - `AccountDetails.razor` – Profile management
    - `UiPersonalisation.razor` – Accessibility and preference settings
    - `Graphs.razor` + `Graphs.razor.cs` – Interactive data visualization
    - **`Diet_Food/`** – Nutrition-related pages
      - `DietQuestionnaire.razor` – Diet recommendation questionnaire
      - `DietResults.razor` – Personalized diet suggestions
      - `FoodLogging.razor` – Daily food entry and tracking
      - `MaintenanceCaloriesQuestionnaire.razor` – Calorie calculator
    - **`Exercise/`** – Training-related pages
      - `ExerciseQuestionnaire.razor` – Exercise recommendation questionnaire
      - `ExerciseResults.razor` – Personalized exercise suggestions
      - `ExerciseLogging.razor` + `.cs` – Workout logging with calorie burn tracking
    - **`Hypertrophy/`** – Resistance training features
      - `Hypertrophy.razor` – Landing page for resistance training
      - `WorkoutSplitGenerator.razor` – Custom workout split creator
      - `WorkoutSplitResults.razor` – Generated workout plans
    - **`AboutMe/`** – Legal and informational pages
      - `AboutMe.razor` – Project information
      - `Privacy Policy.razor`
      - `Terms of Service.razor`
  - **`Shared/`** – Reusable UI components
    - `MacroPieChart.razor` – Macro distribution visualization
    - `Popup.razor` – Modal dialogs
  - **`Layout/`** – Application shell components
    - `ReconnectModal.razor` – Connection loss handling
  - `App.razor` – Root application component
  - `Routes.razor` – Routing configuration
  - `_Imports.razor` – Global using statements

- **`Data/`** – Data layer and business logic
  - **`Models/`** – Business logic and services
    - `AppDbContext.cs` – Entity Framework database context
    - `AuthService.cs` – Authentication and authorization
    - `UserSession.cs` + `UserSessionService.cs` – Session state management
    - `UISettingsService.cs` – User preference management
    - `PasswordHelper.cs` – Password hashing and validation
    - `MaintenanceCalorieCalculator.cs` – BMR and TDEE calculation
    - `DietQuestionnaireState.cs` – Diet recommendation logic
    - `ExerciseQuestionnaireState.cs` – Exercise recommendation logic
    - `WorkoutSplitGeneratorState.cs` – Workout plan generation algorithm
  - **`DatabaseTableModels/`** – Entity models
    - `User.cs` – User entity (not shown but referenced)
    - `FoodType.cs` – Food item definitions
    - `UserFoodItems.cs` – User-food relationships
    - `ExerciseType.cs` – Exercise definitions
    - `UserExercises.cs` – User-exercise relationships
    - `HypertrophyExercise.cs` – Resistance training specifics (referenced)
    - `WeightLogs.cs` – Bodyweight tracking
    - `CalorieLogs.cs` – Nutrition logging
    - `Graph.cs` (referenced) – Visualization metadata

- **`wwwroot/`** – Static web assets
  - CSS stylesheets
  - JavaScript libraries (Chart.js via BlazorBootstrap)
  - Images and icons

- **`Program.cs`** – Application entry point and service registration

### Key Services (Registered in Program.cs)
- **Database**: `AppDbContext` (scoped, Entity Framework Core)
- **Authentication**: `AuthService` (scoped)
- **Session Management**: `UserSessionService` (scoped)
- **UI Settings**: `UISettingsService` (scoped)
- **Calculators**: 
  - `MaintenanceCalorieCalculator` (scoped)
  - `DietQuestionnaireState` (scoped)
  - `ExerciseQuestionnaireState` (scoped)
  - `WorkoutSplitGeneratorState` (scoped)

Files to look for:
- Database context: `Data/Models/AppDbContext.cs`
- Entity models: `Data/DatabaseTableModels/*`
- Business logic: `Data/Models/*State.cs`, `Data/Models/*Service.cs`
- UI pages: `Components/Pages/**/*.razor`
- Shared components: `Components/Shared/*.razor`

## Running locally

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB, Express, or full version)
- Optional: `dotnet-ef` tools for manual migrations
- Visual Studio 2022+ or VS Code (recommended)

### Initial Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/sav-afg/WebsiteFirstDraft.git
   cd WebsiteFirstDraft
   ```

2. **Configure database connection**
   - Copy `appsettings.example.json` to `appsettings.Development.json` (if not already present)
   - Update the connection string for your SQL Server instance:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Server=.;Database=FitnessSaviourDb;Trusted_Connection=True;TrustServerCertificate=True;"
       }
     }
     ```
   - For LocalDB: `Server=(localdb)\\mssqllocaldb;Database=FitnessSaviourDb;Trusted_Connection=True;`

3. **Install EF Core tools** (if not already installed)
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Apply database migrations**
   ```bash
   # From the solution root directory
   dotnet ef database update --context AppDbContext
   ```
   Or migrations can run automatically on startup if configured in `Program.cs`

5. **Run the application**
   ```bash
   dotnet run
   ```
   Or press **F5** in Visual Studio to run with debugging

6. **Access the app**
   - Navigate to `https://localhost:5001` or the port shown in console
   - Create an account to start using the application

### Database Management

**Add a new migration:**
```bash
dotnet ef migrations add MigrationName --context AppDbContext
```

**Update database to latest migration:**
```bash
dotnet ef database update --context AppDbContext
```

**Rollback to a specific migration:**
```bash
dotnet ef database update PreviousMigrationName --context AppDbContext
```

**Remove last migration (if not applied):**
```bash
dotnet ef migrations remove --context AppDbContext
```

### Running Tests
```bash
dotnet test
```

### Development Tips
- The app uses **Blazor Server** with `InteractiveServer` render mode for real-time reactivity
- Check browser console for client-side errors
- Check application output/logs for server-side errors
- Use browser DevTools to inspect SignalR connection status
- Database seeding may occur on first run (check `AppDbContext` configuration)

## Development notes

### Architecture
- **Blazor Server Architecture**: 
  - Uses SignalR for real-time client-server communication
  - Components render on server, UI updates sent over WebSocket
  - Session state maintained on server via scoped services
- **Render Mode**: `@rendermode InteractiveServer` for interactive components
- **Dependency Injection**: 
  - All services registered in `Program.cs`
  - Scoped lifetime for user-specific services (sessions, questionnaires)
  - Transient for stateless utilities

### Code Organization
- **Pages**: Located in `Components/Pages/`, use `.razor` extension
  - Combine markup and logic using `@code` blocks
  - Or use code-behind with `.razor.cs` files
- **Shared Components**: Reusable UI in `Components/Shared/`
- **Layout**: Application shell in `Components/Layout/`
- **Services**: Business logic in `Data/Models/*Service.cs` and `*State.cs` files
- **Data Models**: Entity classes in `Data/DatabaseTableModels/`

### State Management
- **User Sessions**: `UserSessionService` maintains logged-in user state
- **UI Settings**: `UISettingsService` persists user preferences across navigation
- **Questionnaire State**: Scoped services (`DietQuestionnaireState`, etc.) preserve form data
- **Database**: EF Core `AppDbContext` for data persistence

### Database Context
- **Context Class**: `AppDbContext` in `Data/Models/AppDbContext.cs`
- **DbSets**: Define tables (Users, FoodTypes, ExerciseTypes, WeightLogs, etc.)
- **Migrations**: Located in `Migrations/` folder (auto-generated)
- **Relationships**: Configured via Fluent API in `OnModelCreating`

### UI Framework
- **Bootstrap 5**: Primary CSS framework
- **BlazorBootstrap**: Chart components (LineChart, BarChart, PieChart)
- **Custom CSS**: Application-specific styles in `wwwroot/css/`
- **Responsive**: Mobile-first design with Bootstrap grid system

### Security
- **Password Hashing**: Using `PasswordHelper` with secure hashing algorithms
- **Data Encryption**: Email and phone numbers encrypted at rest
- **Input Validation**: Data annotations on models, validation in services
- **SQL Injection Protection**: EF Core parameterized queries

### Best Practices
- **Minimal Changes**: Follow existing patterns when adding features
- **Error Handling**: Use try-catch with user-friendly error messages
- **Async/Await**: All database operations are asynchronous
- **Disposal**: DbContext automatically disposed by DI container
- **Naming Conventions**: PascalCase for public members, camelCase for private fields

## Privacy & Safety

### Health & Safety
- **General Guidance Only**: All recommendations are educational and informational
- **Not Medical Advice**: Does not provide personalized medical, clinical, or diagnostic advice
- **Professional Consultation**: Users are encouraged to consult healthcare professionals before:
  - Starting new diet or exercise programs
  - Making significant lifestyle changes
  - If they have pre-existing medical conditions
- **Safe Recommendations**: Exercise suggestions follow evidence-based, conservative guidelines
- **User Responsibility**: Users are responsible for their own health decisions

### Data Privacy
- **Encryption**: 
  - Passwords hashed with industry-standard algorithms
  - Email addresses and phone numbers encrypted in database
- **Data Collection**: Only collects information necessary for app functionality
- **User Control**: Users can view and manage their personal data
- **Transparency**: Privacy Policy clearly outlines data handling practices
- **No Third-Party Sharing**: User data is not sold or shared with third parties
- **Secure Storage**: All data stored in SQL Server with appropriate security measures

### Accessibility
- Compliant with accessibility best practices
- Multiple options for users with visual, cognitive, and motor differences
- High contrast mode for low vision users
- Dyslexia-friendly font options
- Reduced motion options for vestibular disorders
- Adjustable text size
- Screen reader compatible (semantic HTML)


## Technology Stack

### Frontend
- **Framework**: Blazor Server (.NET 10)
- **Render Mode**: InteractiveServer (server-side rendering with SignalR)
- **UI Library**: Bootstrap 5
- **Charts**: BlazorBootstrap (wrapper for Chart.js)
- **Components**: Razor components (.razor files)

### Backend
- **Runtime**: .NET 10
- **Language**: C#
- **ORM**: Entity Framework Core
- **Database Provider**: Microsoft.EntityFrameworkCore.SqlServer
- **Authentication**: Custom implementation with `AuthService`
- **Session Management**: Scoped services via DI

### Database
- **DBMS**: Microsoft SQL Server
- **Migrations**: EF Core Code-First migrations
- **Data Protection**: Encrypted fields for sensitive data

### Development Tools
- **IDE**: Visual Studio 2022+, VS Code
- **Version Control**: Git, GitHub
- **Package Manager**: NuGet

### Key NuGet Packages
- `Microsoft.AspNetCore.Components.Web`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `BlazorBootstrap` (for charts and UI components)

## Roadmap & Future Enhancements

### Planned Features
- **Mobile App**: Native iOS/Android apps using .NET MAUI
- **Social Features**: 
  - Friend connections and challenges
  - Community leaderboards
  - Progress sharing
- **Advanced Analytics**:
  - Machine learning predictions for weight trends
  - Personalized meal timing recommendations
  - Exercise volume optimization
- **Integration**: 
  - Fitness tracker APIs (Fitbit, Apple Health, Google Fit)
  - Recipe APIs for meal planning
  - Barcode scanning for food logging
- **Enhanced Personalization**:
  - AI-powered adaptive recommendations
  - Dynamic goal adjustments based on progress
  - Automated deload week suggestions
- **Notifications**: 
  - Email/SMS reminders for logging
  - Achievement notifications
  - Weekly progress summaries
- **Export & Reports**:
  - PDF progress reports
  - CSV data export
  - Printable workout plans

### Known Limitations
- Currently single-user local sessions (no concurrent user handling)
- Limited food database (can be expanded)
- Exercise calorie estimates are approximations
- Requires continuous internet connection (Blazor Server)

## Contact / More info
- **Project Type**: A-level Computer Science Coursework
- **Repository**: [https://github.com/sav-afg/WebsiteFirstDraft](https://github.com/sav-afg/WebsiteFirstDraft)
- **Documentation**: Architecture diagrams and development screenshots available in project documentation
- **License**: Educational use

### Contributing
This is an educational project. Feedback and suggestions are welcome via GitHub Issues.

### Acknowledgments
- Built with .NET 10 and Blazor
- Charts powered by BlazorBootstrap and Chart.js
- UI styled with Bootstrap 5
- Developed as coursework demonstrating full-stack web development skills

---

**Note**: This application is intended for educational and general wellness purposes only. Always consult qualified healthcare professionals for personalized medical advice.
