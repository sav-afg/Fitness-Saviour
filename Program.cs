using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebsiteFirstDraft.Components;
using WebsiteFirstDraft.Data.Models;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Components.Web;



namespace WebsiteFirstDraft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<DietQuestionnaireState>();
            builder.Services.AddScoped<ExerciseQuestionnaireState>();

            builder.Services.AddSingleton<UISettingsService>();

            //builder.Services.AddSyncfusionBlazor();
            builder.Services.AddBootstrapBlazor();
            builder.Services.AddBlazorBootstrap();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf0x3WmFZfVhgdl9EY1ZTQ2Y/P1ZhSXxWd0dhXH5acndXQWRUV0B9XEA=");

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();

        }
    }
}
