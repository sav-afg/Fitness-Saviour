using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Plotly.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteFirstDraft.Data.DatabaseTableModels;

namespace WebsiteFirstDraft.Components.Pages.Exercise
{
    public partial class ExerciseLogging
    {

        // Holds all ExerciseType records currently displayed in the UI
        // This list is typically bound to a table or list in the Razor markup
        private List<ExerciseType> exercise_types = new List<ExerciseType>();

        // Temporary object used for creating a new ExerciseType
        // This is usually bound to a form input
        private ExerciseType newExerciseType = new();

        protected override void OnInitialized()
        {
            // Called once when the component is first initialised
            // Loads all ExerciseTypes from the database into memory
            // so they can be displayed in the UI
            exercise_types = [.. Db.exercise_types];
        }

        void Create()
        {
            // Set database column Id as identity so it will automatically increment. Any exceptions are caught so that the application doesnt crash

            try
            {
                // Adds the new exercise type to the database context
                Db.exercise_types.Add(newExerciseType);

                // Persists the change to the database.
                Db.SaveChanges();

                // Adds the newly created exercise type to the local list so the UI updates immediately
                exercise_types.Add(newExerciseType);

                // Optionally log the newly generated Id
                System.Console.WriteLine($"Created ExerciseType with Id {newExerciseType.Id}");

                // Resets the form by creating a new empty object
                newExerciseType = new();
            }
            catch (Exception ex)
            {
                // Minimal error handling - log for debugging
                System.Console.WriteLine($"Error creating ExerciseType: {ex.Message}");
            }
        }

        void Update(ExerciseType exerciseType)
        {
            // Marks the provided exerciseType as modified in the database context
            Db.exercise_types.Update(exerciseType);

            // Saves the updated values to the database
            Db.SaveChanges();
        }

        void Delete(int id)
        {
            // Logs the deletion attempt (useful for debugging)
            System.Console.WriteLine($"Deleting {id}");

            // Finds the exercise type by primary key
            var exerciseType = Db.exercise_types.Find(id);

            // If the record does not exist, exit safely
            if (exerciseType is null) return;

            // Removes the entity from the database
            Db.exercise_types.Remove(exerciseType);
            Db.SaveChanges();

            // Removes the entity from the local list so the UI updates immediately
            exercise_types.Remove(exerciseType);
        }

        // Search functionality
        // Stores the user's search input
        private string searchText = "";

        // Holds the filtered search results
        // This is usually bound to a results list or table
        private List<ExerciseType> myresults = new();

        void Search()
        {
            // Queries the database for exercise types where
            // the ExerciseNames field contains the search text
            // and stores the results in myresults
            myresults = [.. Db.exercise_types.Where(e => e.ExerciseNames.Contains(searchText))];
        }

        // Method that will set the input class as a certain colour based off the value inside the cell
        string IntensityColour(ExerciseType exerciseType)
        {
            return exerciseType.IntensityLevel switch
            {
                "Low" => "faded-green",
                "Moderate" => "faded-yellow",
                "High" => "faded-red",
                _ => string.Empty,
            };
        }

        private bool showPopup;
        double caloriesBurnt = 0;
        string rate = string.Empty;


        //Opens the pop up and captures the value of the current exercise calories burnt per minute attribute inside a string.
        private void OpenPopup(ExerciseType e)
        {
            rate = e.CaloriesBurnedPerMinute.ToString(); 
            showPopup = true;

        }


        private double duration = 0;
        private string errorMessage = String.Empty;

        private void IncrementDuration() => duration++;
        private void DecrementDuration() => duration--;

        private bool display;

        //Displays the total number of calories burnt as an integer
        private async Task DisplayLog(double duration)
        {
            caloriesBurnt = double.Parse(rate) * duration;
            display = true;
            caloriesBurnt = (int)caloriesBurnt;
            
        }

        //Logs the total calories burnt to the user session and database
        private void LogCalories()
        {
            //Updates the user session values
            Session.UserSession.Daily_Calories = (int)caloriesBurnt;
            Session.UserSession.Weekly_Calories = (int)caloriesBurnt;

            try
            {
                // Updates the database values
                var user = Db.Users
        // .Use case-insensitive comparison for username
        .FirstOrDefault(u => EF.Functions.Like(u.Username, Session.UserSession.Username));

                if (user is null)
                {
                    errorMessage = "Error: User not found in database.";
                    StateHasChanged();
                    return;
                }

                user.Daily_Calories -= Session.UserSession.Daily_Calories;
                user.Weekly_Calories -= Session.UserSession.Weekly_Calories;

                var exercise = Db.exercise_types
                    .FirstOrDefault(e => e.CaloriesBurnedPerMinute.ToString() == rate);

                LogUserExercise(user, exercise);

                Db.SaveChanges();

                errorMessage = "exercise logged successfully!";
                showPopup = false;
                display = false;
                StateHasChanged();
            }
            // Catches any exceptions that occur during the database update
            catch (Exception ex)
            {
                errorMessage = $"Error logging exercise: {ex.Message}";
                StateHasChanged();
            }




        }

        // Logs the association between a user and an exercise in the UserExercises table
        private void LogUserExercise(User? user, ExerciseType? exercise)
        {
            if (user == null)
            {
                System.Console.WriteLine("Error: User is null in LogUserExercise");
                errorMessage = "Error: User information is missing.";
                return;
            }

            if (exercise == null)
            {
                System.Console.WriteLine("Error: exercise is null in LogUserExercise");
                errorMessage = "Error: exercise information is missing.";
                return;
            }

            try
            {
                var userExercise = new UserExercises
                {
                    UserId = user.User_id,
                    ExerciseId = exercise.Id,
                    User = user,
                    Exercise = exercise,
                    DateAdded = DateTime.UtcNow
                };

                Db.users_exercises.Add(userExercise);

                // Negative entry to represent burnt calories
                var calorieLog = new CalorieLogs
                {
                    User_id = user.User_id,
                    Calories_Burned = (int)caloriesBurnt,
                    Net_Calories = -(int)caloriesBurnt
                };

                Db.Calorie_Logs.Add(calorieLog);
                Db.SaveChanges();

                System.Console.WriteLine($"Added CalorieLog: UserId={user.User_id}, FoodId={exercise.Id}, Calories_Consumed={(int)caloriesBurnt}");

                System.Console.WriteLine($"Added UserExercise: UserId={user.User_id}, ExerciseId={exercise.Id}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error in LogUserExercise: {ex.Message}");
                errorMessage = $"Error linking exercise: {ex.Message}";
            }
        }

        // Clears the search input and results
        private void ClearSearch()
        {
            searchText = string.Empty;
            myresults.Clear();
        }


    }


}
