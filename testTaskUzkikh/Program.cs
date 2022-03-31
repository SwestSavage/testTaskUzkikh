using Microsoft.EntityFrameworkCore;
using testTaskUzkikh.DbRepository.Implementations;
using testTaskUzkikh.DbRepository.Interfaces;
using testTaskUzkikh.Services;
using testTaskUzkikh.Settings;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositoryContextFactory, RepositoryContextFactory>();

builder.Services.AddScoped<IUserRepository>(
    provider => new UserRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

builder.Services.AddScoped<IUnpRepository>(
    provider => new UnpRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.Configure<MailSettings>(config.GetSection("MailSettings"));

builder.Services.AddHostedService<EmailSenderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var factory = services.GetRequiredService<IRepositoryContextFactory>();

    factory.CreateDbContext(config.GetConnectionString("DefaultConnection")).Database.Migrate();
}

app.Run();
