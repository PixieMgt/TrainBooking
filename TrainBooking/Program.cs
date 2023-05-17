using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using TrainBooking.Models.Entities;
using TrainBooking.Repositories;
using TrainBooking.Repositories.Interfaces;
using TrainBooking.Services;
using TrainBooking.Services.Interfaces;
using TrainBooking.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using TrainBooking.Util.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
        policy.WithOrigins("http://localhost:5173")
              .WithMethods("POST", "DELETE", "GET")
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API Trains",
        Version = "version 1",
        Description = "An API to perform Train operations",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "TrainBooking",
            Email = "trainbookingnoreply@gmail.com",
            Url = new Uri("https://trainbookingpixiembramvh.azurewebsites.net/"),
        },
        License = new OpenApiLicense
        {
            Name = "Train API LICX",
            Url = new Uri("https://example.com/license"),
        }
    });
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.ConfigureApplicationCookie(o => {
    o.ExpireTimeSpan = TimeSpan.FromDays(5);
    o.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
       o.TokenLifespan = TimeSpan.FromHours(3));

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// add view and viewmodel localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder) // translation on views
    .AddDataAnnotationsLocalization(); // translation on viewmodels

//session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.TrainBookingPB";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

// Add Automapper
builder.Services.AddAutoMapper(typeof(Program));

//----> Dependency Injection
builder.Services.AddTransient<IService<Station>, StationService>();
builder.Services.AddTransient<IDAO<Station>, StationDAO>();

builder.Services.AddTransient<IService<Section>, SectionService>();
builder.Services.AddTransient<IDAO<Section>, SectionDAO>();

builder.Services.AddTransient<IService<Ticket>, TicketService>();
builder.Services.AddTransient<IDAO<Ticket>, TicketDAO>();

builder.Services.AddTransient<IService<Booking>, BookingService>();
builder.Services.AddTransient<IDAO<Booking>, BookingDAO>();

builder.Services.AddTransient<IService<AspNetUser>, UsersService>();
builder.Services.AddTransient<IDAO<AspNetUser>, UsersDAO>();

var supportedCultures = new[] { "en", "nl", "de" };

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

var swaggerOptions = new TrainBooking.Options.SwaggerOptions();
builder.Configuration.GetSection(nameof(TrainBooking.Options.SwaggerOptions)).Bind(swaggerOptions);
// Enable middleware to serve generated Swagger as a JSON endpoint.
//RouteTemplate legt het path vast waar de JSON‐file wordt aangemaakt
app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
//// By default, your Swagger UI loads up under / swagger /.If you want to change this, it's thankfully very straight‐forward.
////Simply set the RoutePrefix option in your call to app.UseSwaggerUI in Program.cs:
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
});
app.UseSwagger();

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
