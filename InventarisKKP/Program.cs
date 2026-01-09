using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using InventarisKKP.Data;
using InventarisKKP.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Konfigurasi Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

// Konfigurasi Entity Framework untuk SQL Server
builder.Services.AddDbContext<InventarisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurasi Service Layer
builder.Services.AddSingleton<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<IBarangService, BarangService>();

// Konfigurasi MongoDB Services
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<IMongoKategoriService, MongoKategoriService>();
builder.Services.AddScoped<IMongoBarangService, MongoBarangService>();

// Konfigurasi Authentication dengan Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/AccessDenied/Index";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add cache control middleware
app.Use(async (context, next) =>
{
    // Disable caching for form pages
    if (context.Request.Path.StartsWithSegments("/Kategori/Create") ||
        context.Request.Path.StartsWithSegments("/Barang/Create") ||
        context.Request.Method == "POST")
    {
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";
    }
    
    await next();
});

app.UseRouting();

// Middleware untuk Authentication dan Authorization
app.UseAuthentication();
app.UseAuthorization();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure database is created
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<InventarisDbContext>();
        InventarisKKP.Data.DbInitializer.Initialize(context);
        Console.WriteLine("Database initialized successfully");
        
        // Reset admin password untuk memastikan password benar
        Console.WriteLine("[INFO] Admin password reset on startup");
        var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
        if (adminUser != null)
        {
            adminUser.Password = BCrypt.Net.BCrypt.HashPassword("admin123");
            context.SaveChanges();
            Console.WriteLine("[SUCCESS] Admin password reset to: admin123");
        }
    }
}
catch (Exception ex)
{
    // Log error tapi jangan crash aplikasi
    Console.WriteLine($"Database initialization error: {ex.Message}");
    Console.WriteLine("Application will continue running without database");
}

Console.WriteLine("[INFO] Starting web server...");
Console.WriteLine("[INFO] Application URL: http://0.0.0.0:5500");

app.Run();

Console.WriteLine("[INFO] Application stopped");