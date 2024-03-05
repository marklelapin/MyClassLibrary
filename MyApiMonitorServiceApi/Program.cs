using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.Email;
using MyClassLibrary.Extensions;
using MyClassLibrary.Interfaces;
using System.Net.Http.Headers;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Abstractions;
using MyClassLibrary.Configuration;


//create container
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMongoDBDataAccess>(sp => new MongoDBDataAccess(configuration.GetValue<string>("MongoDatabase:DatabaseName")!
                                                                             , configuration.GetValue<string>("MongoDatabase:ConnectionString")!));
builder.Services.AddTransient<IApiTestDataAccess, ApiTestMongoDataAccess>();
builder.Services.AddTransient<IApiTestRunner, ApiTestRunner>();
builder.Services.AddTransient<IApiTestCollectionFactory, ApiTestCollectionFactory>();
builder.Services.AddTransient<IEmailClient, HotmailClient>();
builder.Services.AddTransient<IAuthorizationMethods, AuthorizationMethods>();
builder.Services.AddTransient<AuthorizationHeaderHandler>();

builder.Services.AddHttpClient("DownstreamApi", options =>
{
   options.BaseAddress = new Uri(configuration.GetValue<string>("DownstreamApi:BaseUrl"));
});

builder.Services.AddHttpClient("AuthenticatedDownstreamApi", options =>
{
    options.BaseAddress = new Uri(configuration.GetValue<string>("DownstreamApi:BaseUrl"));
})
.AddHttpMessageHandler<AuthorizationHeaderHandler>();    


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


 

app.MapGet("/runtestcollections", async (IApiTestCollectionFactory factory, IEmailClient emailClient) =>
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

    if (testOutcome.wasSuccessfull)
    {

        string responseMessage;

        if (testOutcome.testsPassed == testOutcome.testsRun)
        {
            responseMessage = "All Tests Ran and Passed.";
        }
        else
        {
            responseMessage = $"All Tests Ran but only {testOutcome.testsPassed} out of {testOutcome.testsRun} passed.";
        };


        try
        {
            var emailSuccess = await emailClient.SendAsync("ApiMonitor", "default", responseMessage, "");
            if (emailSuccess == false) throw new Exception("EmailClient.SendAsync ran but reported failure.");
        }
        catch
        {
            responseMessage += " EMAIL ALERT FAILED!";
        };

        return Results.Ok(responseMessage);
    }
    else
    {
        try
        {
            var emailSuccess = await emailClient.SendAsync("ApiMonitor", "default", "Api Monitor Error", testOutcome.exception?.Message ?? "No further details.");
            if (emailSuccess == false) throw new Exception("EmailClient.SendAsync ran but reported failure.");

            return Results.Problem(testOutcome.exception?.Message, null, 500);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Test Run Error: {testOutcome.exception?.Message ?? "none"}. \n Email Alert Error: {ex.Message}.", null, 500);
        }


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
