
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.DataAccessMethods;


var builder = WebApplication.CreateBuilder(args);

//TODO - If developing the app further add this in and use for access to test setup page etc.
//builder.ConfigureMicrosoftIdentityWebAuthenticationAndUI("AzureAd");


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMongoDBDataAccess, MongoDBDataAccess>();
builder.Services.AddTransient<IApiTestDataAccess, ApiTestMongoDataAccess>();
builder.Services.AddTransient<IApiTestDataProcessor, ApiTestDataProcessor>();
var app = builder.Build();



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
