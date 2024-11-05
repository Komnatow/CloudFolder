using CloudFolder.Data;
using Microsoft.EntityFrameworkCore;
using CloudFolder.Procedures;
using CloudFolder.Models;
using System.Text.Json;
using System.Web;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText("config.json"));
JsonElement root = jsonDocument.RootElement;
builder.Services.Configure<GlobalVariablesOptions>(global =>
{
    global.InitialFolder = root.GetProperty("InitialFolder").GetString();
    global.currentFolder = global.InitialFolder;
    global.adminFolder = root.GetProperty("AdminOnlyFolder").GetString();
    global.role = "";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<RoleMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DirectoryOperations.CheckDirectory(root.GetProperty("InitialFolder").GetString());

app.Run();
