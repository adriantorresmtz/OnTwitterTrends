
using OnTwitter.Api.HostService;
using OnTwitter.Api.Hubs;
using OnTwitter.Application;
using OnTwitter.Infrastructure;
using Tweetinvi;

var builder = WebApplication.CreateBuilder(args);

#region Load from appsetting

//Get TwitterConfiguration params
var twitterConfig = builder.Configuration.GetSection("twitterSettings");
var ConsumerKey = twitterConfig["ConsumerKey"];
var ConsumerSecret = twitterConfig["ConsumerSecret"];
var BearerToken = twitterConfig["BearerToken"];

#endregion

// Create Singleton for TwitterClient
var appCredentials = new Tweetinvi.Models.ConsumerOnlyCredentials(ConsumerKey, ConsumerSecret)
{
    BearerToken = BearerToken
};
var appClient = new TwitterClient(appCredentials);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Get Services from Infra
builder.Services.AddInfrastructureServices(builder.Configuration);
//Get Services friom Application
builder.Services.AddApplicationServices();

//Back service
builder.Services.AddSingleton<IBackRunningServices, BackRunningServices>();
builder.Services.AddHostedService<BackRunningServices>();


// Set Service for TwitterClient
builder.Services.AddSingleton<TwitterClient>(appClient);

// Add SignalR
builder.Services.AddSignalR();

// Configure CORS
var policyName = "defaultCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName,
        builder =>
        {
            builder
            .SetIsOriginAllowed(_ => true)/// TODO: set specific Origins with policies
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});


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
app.UseCors(policyName);
app.MapHub<TwitterReportHub>("/twitterdatahub");

app.Run();
