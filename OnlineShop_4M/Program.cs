using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop_4M.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();




// позволяет добраться до сессий
builder.Services.AddHttpContextAccessor();

// добавили сервис для сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDefaultIdentity<IdentityUser>().
//    AddEntityFrameworkStores<ApplicationDbContext>();

// for roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddDefaultTokenProviders().AddDefaultUI().
    AddEntityFrameworkStores<ApplicationDbContext>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/*app.Use((context, next) =>
{
    if (!context.Items.ContainsKey("text"))
    {
        context.Items["text"] = "SET KEY AND VALUE";
    }
    else
    {
        context.Items["text"] = "UPDATE VALUE";
    }
    
    return next.Invoke();
}); */

/*
app.Run(context =>
{
    if (context.Request.Cookies.ContainsKey("name"))
    {
        return context.Response.WriteAsync("OK");
    }
    else
    {
        context.Response.Cookies.Append("name", "Ilya");
        return context.Response.WriteAsync("NO");
    }
});
*/

app.UseSession();    // включаем использование сессии

app.MapRazorPages();

/*
app.Run(context =>
{
    if (context.Session.Keys.Contains("session"))
    {
        return context.Response.WriteAsync(context.Session.GetString("session"));
    }
    else
    {
        context.Session.SetString("session","Hello My Session!");
        return context.Response.WriteAsync("OK");
    }
});
*/

app.Run();

