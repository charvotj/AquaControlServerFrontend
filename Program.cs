using AquaControlServerFrontend.Components;
using AquaControlServerFrontend.Services;

namespace AquaControlServerFrontend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddAuthentication();

        // Add custom components
        builder.Services
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IAlertService, AlertService>()
            .AddScoped<IHttpService, HttpService>()
            .AddScoped<ILocalStorageService, LocalStorageService>();

        // configure http client
        builder.Services.AddScoped(x => {
            var apiUrl = new Uri(builder.Configuration["apiUrl"]);
            return new HttpClient() { BaseAddress = apiUrl };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //using (var scope = app.Services.CreateScope())
        //{
        //    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        //    accountService.Initialize().GetAwaiter().GetResult();
        //}

        //app.UseHttpsRedirection();


        app.UseStaticFiles();
        app.UseAntiforgery();

        app.UseAuthentication();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
