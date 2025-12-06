using WEB_MCV.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(35);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ApiService>(client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(apiBaseUrl!);
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<EmployeeSessionService>();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower() ?? "/";
    bool isPublic =
        path == "/" ||
        path.StartsWith("/home") ||
        path.Contains("/auth/") ||
        path.Contains("/employeeportal/");

    if (isPublic)
    {
        await next();
        return;
    }

    var token = context.Session.GetString("jwt");

    if (string.IsNullOrEmpty(token))
    {
        context.Response.Redirect("/Home/Index");
        return;
    }

    await next();
});

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
