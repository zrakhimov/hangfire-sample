using Hangfire;

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

// Add Hangfire Dashboard
app.UseHangfireDashboard();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Sample Job

RecurringJob.AddOrUpdate(() => Console.Write("HelLO!"),Cron.Minutely);


app.Run();