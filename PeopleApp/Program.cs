using Microsoft.EntityFrameworkCore;
using PeopleApp.Data;
using PeopleApp.Data.DefaultData;
using PeopleApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();

var connString = builder.Configuration.GetConnectionString("PeopleConnection");
builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(connString);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
app.UseBlazorFrameworkFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapControllers(); // enables attribute routing
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToController("/manage/{*path:nonfile}", "Index", "Blazor");
    endpoints.MapFallbackToFile("/manage-webassembly/{*path:nonfile}", "index.html");

});

SeedData.SeedDatabase(app);
app.Run();
