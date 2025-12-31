using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteFirstDraft.Data.Models;

namespace WebsiteFirstDraft.Components.Pages
{
    public partial class ExerciseLogging
    {
        private void NavToHypertrophy()
        {
            NavigationManager.NavigateTo("hypertrophy");
        }

        private void NavToExerciseQuestionnaire()
        {
            NavigationManager.NavigateTo("exercisequestionnaire");
        }

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
            exercise_types = Db.exercise_types.ToList();
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
                Console.WriteLine($"Created ExerciseType with Id {newExerciseType.Id}");

                // Resets the form by creating a new empty object
                newExerciseType = new();
            }
            catch (Exception ex)
            {
                // Minimal error handling - log for debugging
                Console.WriteLine($"Error creating ExerciseType: {ex.Message}");
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
            Console.WriteLine($"Deleting {id}");

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
            myresults = Db.exercise_types
                .Where(e => e.ExerciseNames.Contains(searchText))
                .ToList();
        }

        // Method that will set the input class as a certain colour based off the value inside the cell
        string IntensityColour(ExerciseType exerciseType)
        {
            switch (exerciseType.IntensityLevel)
            {
                case "Low":
                    return "faded-green";

                case "Moderate":
                    return "faded-yellow";

                case "High":
                    return "faded-red";

                default:
                    return string.Empty;
            }
        }
    }


}
