using AxiePro;
using AxiePro.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = "server=localhost;user=root;password=koalabear181018;database=axiepro";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));
builder.Services.AddDbContext<DataContext>(
                      dbContextOptions => dbContextOptions
                          .UseMySql(connectionString, serverVersion)
                          // The following three options help with debugging, but should
                          // be changed or removed for production.
                          .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors()
                  );
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAxieApiService, AxieApiService>();
builder.Services.AddScoped<IAxieDataFactory, AxieDataFactory>();
builder.Services.AddScoped<IBattlesReportsService, BattlesReportsService>();
builder.Services.AddScoped<IPaymentProcessService, PaymentProcessService>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTc3Mjc5QDMxMzkyZTM0MmUzMGdkc2pTcVR1cHUyczFtM1BrMGlPdWhSNGozY1RsOU5LL0xwYjg0OXNiWEU9");
var app = builder.Build();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
