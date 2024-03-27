var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Guardar",
    pattern: "{area:exists}/{controller=Guardar}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Menu",
    pattern: "{area:exists}/{controller=Menu}/{action=Index}/{id?}");
//name: "default",
//pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Loging",
    pattern: "{controller=Loging}/{action=Index}/{id?}");

app.Run();
