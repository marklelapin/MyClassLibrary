using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using MyApiMonitorService.Interfaces;
using MyApiMonitorService.Models;
using MyClassLibrary.DataAccessMethods;
using Quartz;


var builder = WebApplication.CreateBuilder(args);




TokenAcquirerFactory tokenAcquirerFactory = TokenAcquirerFactory.GetDefaultInstance();

tokenAcquirerFactory.Services.AddDownstreamApi(
                                    "DownstreamApi"
                                    , tokenAcquirerFactory.Configuration.GetSection("DownstreamApi"));


// By default, you get an in-memory token cache.
// For more token cache serialization options, see https://aka.ms/msal-net-token-cache-serialization

// Resolve the dependency injection.
var serviceProvider = tokenAcquirerFactory.Build();

//builder.Add(tokenAcquirerFactory.Services.AddDownstreamApi(
//                                        "DownstreamApi"
//                                        , tokenAcquirerFactory.Configuration.GetSection("DownstreamApi")));
                                

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IMongoDBDataAccess, MongoDBDataAccess>();
builder.Services.AddSingleton<IApiTestingDataAccess, ApiTestingMongoDataAccess>();
builder.Services.AddTransient<IApiTestRunner>(s => new ApiTestRunner(s.GetService<IApiTestingDataAccess>(),s.GetService<IDownstreamApi>()));
builder.Services.AddTransient<IApiTestCollectionFactory, ApiTestCollectionFactory>();


builder.Services.AddQuartz(opts =>
{
    opts.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzHostedService(opts =>
{
    opts.WaitForJobsToComplete = true;
});

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
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

await app.RunAsync();
