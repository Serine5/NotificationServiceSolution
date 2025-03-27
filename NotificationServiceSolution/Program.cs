using ApplicationLayer.IProviders;
using ApplicationLayer.IServices;
using ApplicationLayer.Models;
using ApplicationLayer.Providers;
using ApplicationLayer.Services;
using ApplicationLayer.Validators;
using DAL.Context;
using DAL.Repositories;
using FluentValidation;
using Integrations.Providers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>(true);

// Add services to the container.
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IValidator<SendNotificationDto>, SendNotificationDtoValidator>();
builder.Services.AddMemoryCache();

// Register mock integration providers.
builder.Services.AddScoped<ISmsProvider, MockSmsProvider>();
builder.Services.AddScoped<IPushProvider, MockPushProvider>();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpClient<IEmailProvider, MockEmailProvider>();
}
else
{
    builder.Services.AddTransient<IEmailProvider, SmtpMailIntegration>();
}

builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
