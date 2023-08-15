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

    (bool wasSuccessfull, Exception? exception, int testsPassed, int testsRun) testOutcome;

    try
    {
        List<ApiTestCollection> testCollections = factory.GenerateTestCollections();

        testOutcome = factory.ExecuteTestCollections(testCollections);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, null, 500);
    }

    if (testOutcome.wasSuccessfull && testOutcome.testsPassed == testOutcome.testsRun)
    {
        return Results.Ok("All Tests Ran and Passed.");
    }
    else if (testOutcome.wasSuccessfull && testOutcome.testsPassed != testOutcome.testsRun)
    {
        return Results.Ok($"All Tests Ran but only {testOutcome.testsPassed} out of {testOutcome.testsRun} passed.");
    }
    else
    {
        return Results.Problem(testOutcome.exception?.Message, null, 500);
    }
})
.WithName("RunTestCollections");

app.MapGet("/runavailabilitytests", (IApiTestCollectionFactory factory) =>
{
    (bool wasSuccessfull, Exception? exception, int testsPassed, int testsRun) testOutcome;

    try
    {
        List<ApiTestCollection> testCollections = factory.GenerateAvailabilityTestCollections();

        testOutcome = factory.ExecuteTestCollections(testCollections);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message, null, 500);
    }

    if (testOutcome.wasSuccessfull)
    {
        return Results.Ok("Availability Tests Ran To Completion");
    }
    else
    {
        return Results.Problem(testOutcome.exception?.Message, null, 500);
    }
}).WithName("RunAvailabilityTests");





app.Run();