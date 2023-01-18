using CMS.Service.Logging;
using Serilog;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// adding serilog as loggingService
builder.Host.UseSerilog((hostingContext, loggingConfiguration) => loggingConfiguration
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "CMS_Application")
    .Enrich.WithProperty("MachineName", Environment.MachineName)
    .Enrich.WithProperty("CurrentManagedThreadId", Environment.CurrentManagedThreadId)
    .Enrich.WithProperty("OSVersion", Environment.OSVersion)
    .Enrich.WithProperty("Version", Environment.Version)
    .Enrich.WithProperty("UserName", Environment.UserName)
    .Enrich.WithProperty("ProcessId", Process.GetCurrentProcess().Id)
    .Enrich.WithProperty("ProcessName", Process.GetCurrentProcess().ProcessName)
    .WriteTo.File(formatter: new CustomTextFormatter(), path: Path.Combine(hostingContext.HostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}", $"cms_core_ng_{DateTime.Now:yyyyMMdd}.txt"))
    .ReadFrom.Configuration(hostingContext.Configuration)
);

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//
//    }catch(Exception ex)
//    {
//        Log.Error($"An error occurred while seeding the database {ex.Message} {ex.StackTrace} {ex.InnerException} {ex.Source}");
//    }
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
