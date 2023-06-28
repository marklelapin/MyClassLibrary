using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.Extensions;
using Quartz;
using System.Net.Http.Headers;

//create container
var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddQuartz(opts =>
{
    opts.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzHostedService(opts =>
{
    opts.WaitForJobsToComplete = true;
});


//build container
var app = builder.Build();


var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

var job = JobBuilder.Create<ApiTestJob>()
    .WithIdentity("TestJob")
    .Build();

var trigger = TriggerBuilder.Create()
    .WithIdentity("TestJobTrigger")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInMinutes(5)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

await app.RunAsync();
