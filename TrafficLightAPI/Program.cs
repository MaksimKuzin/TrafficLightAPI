using DatabaseImplements.BusinessLogic;
using DatabaseImplements.Contracts;
using DatabaseImplements.Models;
using Microsoft.EntityFrameworkCore;
using TrafficLightAPI.BusinessLogic;
using TrafficLightAPI.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TLDbContext>();

builder.Services.AddScoped<ISequenceStorage, SequenceStorage>();
builder.Services.AddScoped<ISequenceLogic, SequenceLogic>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TLDbContext>();
    dbContext.Database.EnsureCreated();
}

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
