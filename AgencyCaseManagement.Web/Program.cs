using AgencyCaseManagement.Application.Services;
using AgencyCaseManagement.Domain.Entities;
using AgencyCaseManagement.Infrastructure;
using AgencyCaseManagement.Infrastructure.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApp", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IMeetingService, MeetingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var caseSeeder = new CaseSeeder(
        context, 
        scope.ServiceProvider.GetRequiredService<ILogger<CaseSeeder>>()
    );

    var clientSeeder = new ClientSeeder(
        context,
        scope.ServiceProvider.GetRequiredService<ILogger<ClientSeeder>>()
        );

    var meetingSeeder = new MeetingSeeder(
        context,
        scope.ServiceProvider.GetRequiredService<ILogger<MeetingSeeder>>()
        );

    var userSeeder = new UserSeeder(
        context,
        scope.ServiceProvider.GetRequiredService<ILogger<UserSeeder>>()
        );

    var seedBase = Path.Combine(
        AppContext.BaseDirectory,
        "Data"
        );


    await caseSeeder.CaseSeedAsync(Path.Combine(seedBase, "Cases-150.json"));
    await clientSeeder.ClientSeedAsync(Path.Combine(seedBase, "Clients-100.json"));
    await meetingSeeder.MeetingSeedAsync(Path.Combine(seedBase, "Meetings-100.json"));
    await userSeeder.UserSeedAsync(Path.Combine(seedBase, "Users-50.json"));
}

app.UseCors("AllowApp");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
