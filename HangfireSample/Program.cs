using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add Hangfire services
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add the processing server as IHostedServer
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseStaticFiles();



app.UseRouting();
app.UseAuthorization();
// Add Hangfire Dashboard
app.UseHangfireDashboard("/hangfire",new DashboardOptions()
{
    DashboardTitle = "Title 2.0",
    AsyncAuthorization = new [] {new MyAuthorizationFilter()}
});
app.MapRazorPages();

// Sample Job

RecurringJob.AddOrUpdate(() => Console.Write("HelLO!"),Cron.Minutely);


app.Run();


public class MyAuthorizationFilter : Hangfire.Dashboard.IDashboardAsyncAuthorizationFilter
{
    public async Task<bool> AuthorizeAsync(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var userManager = httpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        
        // Get the current user
        var user = await userManager.GetUserAsync(httpContext.User);
        
        // Check if the user has "Admin" role
        return await userManager.IsInRoleAsync(user, "Admin");
    }
}