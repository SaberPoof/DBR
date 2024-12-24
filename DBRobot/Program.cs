using DBRobot.Data;
using DBRobot.Models;
using DBRobot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PgContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("pgsqlConnection")));

builder.Services.AddControllers();

var key = "sec";
var iv = "sec";
var protectService = new ProtectService(key, iv);

var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.enc");
var authenticationManager = new AuthenticationManager(filePath, protectService);
var (login, password) = authenticationManager.GetAuthenticationData();

builder.Services.AddSingleton(authenticationManager);
builder.Services.AddSingleton(new Authen(login, password));



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
