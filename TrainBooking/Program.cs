using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using TrainBooking.Models.Data;
using TrainBooking.Models.Entities;
using TrainBooking.Repositories;
using TrainBooking.Repositories.Interfaces;
using TrainBooking.Services;
using TrainBooking.Services.Interfaces;
using TrainBooking.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// add view and viewmodel localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder) // translation on views
    .AddDataAnnotationsLocalization(); // translation on viewmodels

//session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.TrainBookingPB";
    options.IdleTimeout = TimeSpan.FromMinutes(1);
});

// Add Automapper
builder.Services.AddAutoMapper(typeof(Program));

//----> Dependency Injection
builder.Services.AddTransient<IService<Station>, StationService>();
builder.Services.AddTransient<IDAO<Station>, StationDAO>();

builder.Services.AddTransient<IService<Section>, SectionService>();
builder.Services.AddTransient<IDAO<Section>, SectionDAO>();

builder.Services.AddTransient<IService<AspNetUser>, UsersService>();
builder.Services.AddTransient<IDAO<AspNetUser>, UsersDAO>();

var supportedCultures = new[] { "en", "nl" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures) // used for formatting culture dependant data (dates, currencies, etc)
    .AddSupportedUICultures(supportedCultures); // used for formatting strings (using resource files)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Culture from the HttpRequest
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseRouting();

//add session
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
