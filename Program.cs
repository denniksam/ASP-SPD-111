using ASP_SPD_111.Data;
using ASP_SPD_111.Middleware;
using ASP_SPD_111.Services.Hash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// додаємо до конфігурації файл з даними підключення БД
builder.Configuration.AddJsonFile("dbsettings.json");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Реєструємо власні сервіси ...
builder.Services.AddSingleton<IHashService, Sha1HashService>();
// ... та контекст даних
String? connectionString = 
    builder
    .Configuration
    .GetConnectionString("PlanetScale");

MySqlConnection connection = new(connectionString);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection),
        serverOptions =>
            serverOptions
            .MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName,
                schema: "ASP_SPD_111")
            .SchemaBehavior(
                MySqlSchemaBehavior.Translate,
                (schema, table) => $"{schema}_{table}")
    )
);


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseSession();

// включення нашого Middleware у ланцюг
app.UseMiddleware<AuthSessionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
