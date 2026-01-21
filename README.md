# 🏋️ Fitness Saviour

A comprehensive full-stack fitness web application built with **Blazor Server** and **C#**. Track your nutrition, log exercises, visualize progress with interactive charts, and get personalized diet and workout recommendations.

> **🎓 Academic Project** - Built for coursework demonstration  
> **🚀 Tech Stack:** .NET 7, Blazor Server, Entity Framework Core, In-Memory Database

---

## 📋 Table of Contents
- [Features](#-features)
- [Quick Start](#-quick-start)
- [Tech Stack](#-tech-stack)
- [Architecture](#-architecture)
- [Database Schema](#-database-schema)
- [Test Accounts](#-test-accounts)
- [Screenshots](#-screenshots)
- [Development](#-development)
- [Contributing](#-contributing)

---

## ✨ Features

### 📊 Progress Tracking & Visualization
- **8 Interactive Graphs** powered by Chart.js and Blazor Bootstrap
  - Bodyweight over time (8 weeks of data)
  - Weekly weight change trends
  - Daily calorie intake vs. maintenance target
  - Calorie surplus/deficit visualization
  - Macro distribution pie charts
  - Daily macro intake breakdown
  - Exercise calories burned tracking
  - Exercise type frequency analysis

### 🍽️ Nutrition Management
- **Food Logging System**
  - Database of 110+ food items with accurate nutritional data
  - Search and filter functionality
  - Macro tracking (carbs, protein, fats)
  - Calorie calculations per gram
  - Historical meal logging

### 🏃 Exercise Tracking
- **Comprehensive Exercise Library**
  - 110+ exercises across 7 categories
  - Color-coded intensity levels:
    - 🟢 Low (Green)
    - 🟡 Medium/Moderate (Yellow)
    - 🔴 High (Red)
  - Calorie burn estimates per minute
  - Exercise type filtering

### 🎯 Smart Recommendations
- **Diet Questionnaire**
  - Personalized diet recommendations
  - Goal-based filtering (weight loss, muscle gain, maintenance)
  - Dietary preference accommodations
  - Budget and time constraint analysis

- **Exercise Questionnaire**
  - Customized workout suggestions
  - Experience level assessment
  - Location preference matching
  - Intensity recommendations

- **Workout Split Generator**
  - Personalized training splits
  - Muscle group distribution
  - Recovery time optimization

### 🧮 Calculators
- **Maintenance Calorie Calculator**
  - BMR calculation
  - Activity level adjustment
  - TDEE estimation
  - Goal-based calorie targets

### ♿ Accessibility Features
- **Comprehensive UI Settings**
  - High contrast mode
  - Dyslexia-friendly fonts
  - Larger text sizes
  - Reduced animations
  - Minimal interface option
  - Customizable tracking preferences

### 👤 User Management
- **Authentication & Authorization**
  - Secure password hashing
  - Session management
  - User profiles
  - Login streak tracking

---

## 🚀 Quick Start

### Prerequisites
- **.NET 7 SDK** or higher
- Visual Studio 2022 or VS Code (optional)
- Web browser (Chrome, Firefox, Edge recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/sav-afg/WebsiteFirstDraft.git
   cd WebsiteFirstDraft
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Navigate to the app**
   ```
   https://localhost:5001
   or
   http://localhost:5000
   ```

### 🔐 Login with Test Accounts

| Username | Password | Profile |
|----------|----------|---------|
| `john_fitness` | `Password123!` | 85kg, Fat Loss Goal, 15-day streak |
| `sarah_health` | `Password123!` | 68kg, Maintenance Goal, 30-day streak |
| `mike_athlete` | `Password123!` | 92kg, Bulking Goal, 45-day streak |

> 💡 **Tip:** Each user has 56 days of weight logs and 30 days of meal/exercise data pre-seeded for graph visualization!

---

## 🛠️ Tech Stack

### Frontend
- **Blazor Server** (.NET 7)
- **Razor Components** for UI
- **Bootstrap 5** for responsive design
- **Blazor Bootstrap** for chart components
- **Chart.js** for data visualization
- **Custom CSS** for styling

### Backend
- **C# 11** (.NET 7)
- **ASP.NET Core** 7.0
- **Entity Framework Core** 7.0.20
- **In-Memory Database** (no SQL Server required!)

### Key Libraries
```xml
<PackageReference Include="Blazor.Bootstrap" Version="1.10.5" />
<PackageReference Include="BootstrapBlazor" Version="7.11.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="7.0.20" />
```

---

## 🏗️ Architecture

### Project Structure
```
WebsiteFirstDraft/
├── Components/
│   ├── Layout/              # Layout components (MainLayout, NavMenu)
│   ├── Pages/               # Blazor pages/routes
│   │   ├── Diet_Food/       # Food logging, diet questionnaire
│   │   ├── Exercise/        # Exercise logging, exercise questionnaire
│   │   ├── Hypertrophy/     # Workout split generator
│   │   ├── Shared/          # Reusable components (charts, modals)
│   │   ├── AboutMe/         # Privacy policy, terms of service
│   │   ├── Home.razor       # Landing page with dashboard
│   │   ├── Graphs.razor     # Data visualization hub
│   │   └── AccountDetails.razor
│   ├── App.razor            # Root component
│   ├── Routes.razor         # Routing configuration
│   └── _Imports.razor       # Global using statements
├── Data/
│   ├── DatabaseTableModels/ # Entity models
│   │   ├── User.cs
│   │   ├── CalorieLogs.cs
│   │   ├── WeightLog.cs
│   │   ├── ExerciseType.cs
│   │   └── FoodType.cs
│   ├── Models/              # Service classes
│   │   ├── AppDbContext.cs
│   │   ├── AuthService.cs
│   │   ├── UserSessionService.cs
│   │   └── [Questionnaire States]
│   └── DatabaseSeeder.cs    # Seed data generator
├── Utilities/
│   └── ColorUtility.cs      # Chart color utilities
├── Pages/
│   └── _Host.cshtml         # Blazor Server entry point
├── wwwroot/                 # Static assets (CSS, JS, images)
└── Program.cs               # Application startup
```

### Design Patterns
- **Dependency Injection**: Services registered in `Program.cs`
- **Repository Pattern**: `AppDbContext` for data access
- **Component-Based Architecture**: Reusable Blazor components
- **State Management**: Scoped and singleton services
- **MVC Pattern**: Separation of concerns with code-behind files

---

## 💾 Database Schema

### Core Tables

#### **Users**
Stores user accounts, preferences, and aggregate metrics
```csharp
- User_id (PK)
- Username, PasswordHash, Email, Phone_Number
- Role, Login_Streak, Created_At
- Body_Weight, Maintenance_Calories
- Daily_Calories, Daily_Carbs, Daily_Protein, Daily_Fat
- UI Preferences (High_Contrast_Mode, Dyslexia_Friendly_Font, etc.)
```

#### **Weight_Logs**
Tracks weight measurements over time
```csharp
- WeightLog_Id (PK)
- UserId (FK)
- LogDate, Weight
```

#### **Calorie_Logs**
Records daily nutrition and exercise
```csharp
- CalorieLog_Id (PK)
- User_id (FK)
- Log_Date
- Calories_Consumed, Calories_Burned, Net_Calories
- Carbs_Consumed, Protein_Consumed, Fat_Consumed
- Calories_From_Carbs, Calories_From_Protein, Calories_From_Fats
```

#### **Food_items**
Database of foods with nutritional information
```csharp
- Food_Id (PK)
- Food_Name, Food_Type
- Calories_Per_Gram, Carbs_Per_Gram, Protein_Per_Gram, Fat_Per_Gram
```

#### **exercise_types**
Library of exercises with calorie burn data
```csharp
- Exercise_Id (PK)
- ExerciseNames, ExerciseTypes
- CaloriesBurnedPerMinute, IntensityLevel
```

### Seed Data
- **3 User Accounts** with full profiles
- **110 Food Items** across all macros
- **110 Exercise Types** across 7 categories
- **56 Days** of weight logs per user
- **30 Days** of calorie/macro logs per user

---

## 👥 Test Accounts

### John Fitness (Weight Loss Journey)
```
Username: john_fitness
Password: Password123!
Stats:
  - Weight: 85kg (trending down)
  - Goal: Fat loss
  - Daily Calories: ~2650 kcal
  - Macros: 300g carbs, 150g protein, 80g fat
  - Exercise: 450 kcal/day burned
```

### Sarah Health (Maintenance)
```
Username: sarah_health
Password: Password123!
Stats:
  - Weight: 68kg (stable)
  - Goal: Health & wellness
  - Daily Calories: ~1850 kcal
  - Macros: 200g carbs, 120g protein, 65g fat
  - Exercise: 350 kcal/day burned
  - UI: High contrast mode enabled
```

### Mike Athlete (Muscle Gain)
```
Username: mike_athlete
Password: Password123!
Stats:
  - Weight: 92kg (trending up)
  - Goal: Bulking
  - Daily Calories: ~3200 kcal
  - Macros: 400g carbs, 180g protein, 90g fat
  - Exercise: 600 kcal/day burned
  - UI: Dyslexia-friendly font, larger text
```

---

## 📸 Screenshots

### Dashboard
- Real-time macro pie chart
- Summary statistics
- Quick navigation tiles
- Motivational quotes based on goals

### Graphs Page
- 8 interactive charts
- Historical data visualization
- Trend analysis
- Export capabilities

### Food Logging
- Searchable database (110+ foods)
- Quick macro calculations
- Meal history
- Daily totals

### Exercise Logging
- Color-coded intensity levels
- Calorie burn estimates
- Exercise search/filter
- Workout history

---

## 🔧 Development

### Build & Run
```bash
# Clean build
dotnet clean
dotnet build

# Run in development mode
dotnet run

# Run with watch (hot reload)
dotnet watch run

# Build for production
dotnet publish -c Release
```

### Database Management

The application uses an **in-memory database** that resets on every restart. This is perfect for:
- ✅ Development and testing
- ✅ Demonstrations without setup
- ✅ No SQL Server dependency

To check database status, navigate to:
```
/dbstatus
```

### Switching to SQL Server (Optional)

1. Update `Program.cs`:
```csharp
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

2. Add connection string to `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=FitnessTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. Run migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Code Structure Guidelines

#### Blazor Components
- Use code-behind files (`.razor.cs`) for complex logic
- Keep `.razor` files focused on UI/markup
- Inject services via `@inject` directive

#### Services
- Register as Scoped for per-request state
- Register as Singleton for app-wide state
- Use constructor injection

#### Data Access
- Always use `AppDbContext` for database operations
- Prefer async methods (`ToListAsync`, `FirstOrDefaultAsync`)
- Dispose contexts properly (handled by DI)

---

## 🐛 Known Issues

1. **Graphs.razor Binding Warning** (Cosmetic)
   - RZ9991 warning about bind attribute inference
   - Does not affect functionality
   - Resolved with build server restart

2. **In-Memory Data Persistence**
   - Database resets on application restart
   - By design for development/demo purposes

3. **.NET 7 End of Support**
   - Warning: NETSDK1138
   - Consider upgrading to .NET 8 LTS for production

---

## 📚 Additional Documentation

- [SEEDED_USERS_README.md](SEEDED_USERS_README.md) - Detailed user account info
- [IN_MEMORY_DATABASE_FIX.md](IN_MEMORY_DATABASE_FIX.md) - Database migration notes
- [DATABASE_EXPANSION_SUMMARY.md](DATABASE_EXPANSION_SUMMARY.md) - Food/exercise data details

---

## 🤝 Contributing

This is an academic project, but suggestions are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is for educational purposes.

---

## 🙏 Acknowledgments

- **Bootstrap** for responsive UI framework
- **Blazor Bootstrap** for chart components
- **Chart.js** for data visualization
- **Entity Framework Core** for ORM
- **USDA FoodData Central** for nutritional data accuracy

---

## 📞 Contact

**Repository:** [https://github.com/sav-afg/WebsiteFirstDraft](https://github.com/sav-afg/WebsiteFirstDraft)  
**Branch:** NET7

---

## 🎯 Future Enhancements

- [ ] Progressive Web App (PWA) support
- [ ] Export data to CSV/PDF
- [ ] Social features (friend challenges)
- [ ] Meal planning assistant
- [ ] Recipe database integration
- [ ] Mobile app (Blazor Hybrid/MAUI)
- [ ] REST API for third-party integrations
- [ ] Advanced analytics and insights
- [ ] AI-powered recommendations
- [ ] Barcode scanning for food logging

---

**Built with ❤️ using Blazor and .NET 7**
- Does not provide prescriptive medical or clinical advice.
- Provides general, safe exercise suggestions and prompts to consult healthcare professionals when necessary.


## Contact / More info
- Project developed as coursework for A-level Computer Science.
- For architecture diagrams and development screenshots, refer to the "Application Development Screenshots" document bundled with the repo.
