
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.DataAccessMethods;


var builder = WebApplication.CreateBuilder(args);

//TODO - If developing the app further add this in and use for access to test setup page etc.
//builder.ConfigureMicrosoftIdentityWebAuthenticationAndUI("AzureAd");


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IMongoDBDataAccess, MongoDBDataAccess>();
builder.Services.AddSingleton<IApiTestingDataAccess, ApiTestingMongoDataAccess > ();
//TODO - Add in regular refresh of page - probably simpler way of doing this.
//builder.Services.AddQuartz(opts =>
//{
//    opts.UseMicrosoftDependencyInjectionJobFactory();
//});
//builder.Services.AddQuartzHostedService(opts =>
//{
//    opts.WaitForJobsToComplete = true;
//});

var app = builder.Build();

//TODO - add in regular refresh of page - probably simpler way of doing this.
//var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
//var scheduler = await schedulerFactory.GetScheduler();

//var job = JobBuilder.Create<ApiTestJob>()
//    .WithIdentity("TestJob")
//    .Build();

//var trigger = TriggerBuilder.Create()
//    .WithIdentity("TestJobTrigger")
//    .StartNow()
//    .WithSimpleSchedule(x=>x
//        .WithIntervalInMinutes(5)
//        .RepeatForever())
//    .Build();

//await scheduler.ScheduleJob(job, trigger);



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //TODO - Add in better error page.
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseCookiePolicy();
app.UseRouting();

//app.UseAuthentication();    
//app.UseAuthorization();

app.MapRazorPages();
//app.MapControllers();

await app.RunAsync();
