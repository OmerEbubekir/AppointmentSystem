using Bussiness.Interfaces;
using Bussiness.Services;
using Data.Context;
using Data.Entitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication("MyCookieAuth") 
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
    });


builder.Services.AddSession();


var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();

    if (!context.Users.Any(u => u.Email == "admin@demo.com"))
    {
        var hasher = new PasswordHasher<User>();
        var adminUser = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@demo.com",
            Role = "Admin"
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!"); // düz þifre

        context.Users.Add(adminUser);
        context.SaveChanges();
    }
}


app.Run();
