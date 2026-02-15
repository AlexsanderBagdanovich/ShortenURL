using ShortenURL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())                                   // Создаем область для сервисов, чтобы получить DbContext. EnsureCreated автоматически создаст базу данных и таблицы,
{                                                                                // если их ещё нет. Это облегчает запуск проекта без ручной настройки БД
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Links}/{action=Index}/{id?}");                         // Задаем "default route", чтобы можно было обращаться к /Links/Index по умолчанию и использовать id как опциональный параметр

app.Run();
