
using Autofac.Core;
using OnTwitter.Api.HostService;
using OnTwitter.Application;
using OnTwitter.Infrastructure;
using OnTwitter.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddHostedService<TwitterReader>();
builder.Services.AddSingleton<IBackRunningServices, BackRunningServices>();
//builder.Services.AddHostedService(sp => sp.GetRequiredService<BackRunningServices>());
builder.Services.AddHostedService<BackRunningServices>();

//Get Services for Infra
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();



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
