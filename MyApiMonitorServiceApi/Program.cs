using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.Extensions;
using System.Net.Http.Headers;

//create container
var builder = WebApplication.CreateBuilder(args);

//TODO - Add Authentication to ApiMonitor

//
string token = builder.GetTokenFromAzureAdB2cClientCredentialsFlow();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMongoDBDataAccess, MongoDBDataAccess>();
builder.Services.AddTransient<IApiTestDataAccess, ApiTestMongoDataAccess>();
builder.Services.AddTransient<IApiTestRunner, ApiTestRunner>();
builder.Services.AddTransient<IApiTestCollectionFactory, ApiTestCollectionFactory>();

builder.Services.AddHttpClient("DownstreamApi", options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("DownstreamApi:BaseUrl"));
    options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
});


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();


app.MapGet("/runtestcollections", (IApiTestCollectionFactory factory) =>
{
    try
    {
        List<ApiTestCollection> testCollections = factory.GenerateTestCollections();

        factory.ExecuteTestCollections(testCollections);
    }
    catch (Exception ex)
    {
        Results.Problem(ex.Message, null, 500);
    }

    Results.Ok("Test Collections Ran To Completion.");

})
.WithName("RunTestCollections");

app.Run();