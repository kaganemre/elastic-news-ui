using ElasticNews.Application.Services;
using ElasticNews.Infrastructure;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddInfrastructure();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<INewsIndexingJob>(
    "news-indexing-job",
    job => job.FetchAndIndexNewsAsync(),
    Cron.Hourly);

BackgroundJob.Enqueue<INewsIndexingJob>(job => job.FetchAndIndexNewsAsync());


app.UseAuthorization();

app.MapStaticAssets();
app.MapHangfireDashboard();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
