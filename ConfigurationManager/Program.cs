using ConfigurationManager.ConfigurationManager.API.Hubs;
using ConfigurationManager.ConfigurationManager.Application.Services;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ConfigurationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConfigurationRepository<Configuration>, ConfigurationRepository>();
builder.Services.AddScoped<IRepository<BaseConfigurationVersion>, ConfigurationVersionRepository>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:63342") // Замените на ваш клиентский домен
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddMediatR(typeof(ConfigurationHub).Assembly);
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ConfigurationHub>("/configurationHub");
    endpoints.MapControllers();
});

app.Run();
