using ConfigurationManager.ConfigurationManager.Application.Services;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ConfigurationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IConfigurationRepository<Configuration>, ConfigurationRepository>();
builder.Services.AddScoped<IRepository<BaseConfigurationVersion>, ConfigurationVersionRepository>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

builder.Services.AddSignalR();


builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //endpoints.MapHub<ConfigurationHub>("/configurationHub"); // Map SignalR Hub
});

app.Run();
