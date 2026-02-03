using Microsoft.EntityFrameworkCore;
using Web_App_VM_Management_System.AppContext;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
string sqlServerConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<VMdbContext>(options => options.UseSqlServer(sqlServerConnectionStr));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = false;
})
   .AddEntityFrameworkStores<VMdbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=index}/{id?}"
    );
});

app.MapRazorPages();

// Create roles (if not already created)
var serviceProvider = builder.Services.BuildServiceProvider();
using (var scope = serviceProvider.CreateScope())
{
    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //CreateRolesIfNotExists(roleManager, "Admin");
    //CreateRolesIfNotExists(roleManager, "User");
}

app.Run();

// void CreateRolesIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
//{
//    if (!roleManager.RoleExistsAsync(roleName).Result)
//    {
//        roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
//    }
//}
