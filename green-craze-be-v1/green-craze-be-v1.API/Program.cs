using green_craze_be_v1.API.Middlewares;
using green_craze_be_v1.Application;
using green_craze_be_v1.Application.Common.SignalR;
using green_craze_be_v1.Infrastructure;
using Hangfire;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
// Add configurations
builder.Configuration
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
#pragma warning disable CS0612 // Type or member is obsolete
builder.Services.AddApplicationLayer(builder.Configuration);
#pragma warning restore CS0612 // Type or member is obsolete
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request middleware pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();
app.UseMiddleware<ExceptionMiddleware>();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AppHub>("/app-hub");
app.MigrateDatabase();
app.UseHangfireDashboard("/dashboard");
app.Run();