using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebsiteFirstDraft.Components;
using WebsiteFirstDraft.Data.Models;
using WebsiteFirstDraft.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace WebsiteFirstDraft
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Use In-Memory Database instead of SQL Server
            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseInMemoryDatabase("FitnessTrackerDB"));


            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            builder.Services.AddScoped<DietQuestionnaireState>();
            builder.Services.AddScoped<ExerciseQuestionnaireState>();

            // Added service for maintenance calorie calculator
            builder.Services.AddScoped<MaintenanceCalorieCalculator>();

            builder.Services.AddScoped<AuthService>();

            // Register UserSessionService as a singleton service because it holds user-specific data.
            builder.Services.AddSingleton<UserSessionService>();

            builder.Services.AddSingleton<UISettingsService>();

            builder.Services.AddScoped<WorkoutSplitGeneratorState>();


            //builder.Services.AddSyncfusionBlazor();
            builder.Services.AddBootstrapBlazor();
            builder.Services.AddBlazorBootstrap();


            var app = builder.Build();

            // Seed the in-memory database with sample data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                DatabaseSeeder.SeedDatabase(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found");
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf0x3WmFZfVhgdl9EY1ZTQ2Y/P1ZhSXxWd0dhXH5acndXQWRUV0B9XEA=");

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();

        }
    }
}
