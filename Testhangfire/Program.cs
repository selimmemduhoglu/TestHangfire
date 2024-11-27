using Hangfire;
using Microsoft.Data.SqlClient;
using Testhangfire.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IJobService, JobService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

dynamic test=builder.Configuration.GetConnectionString("HangfireConnection");
string testString=builder.Configuration.GetConnectionString("HangfireConnection");


try
{
    using (var connection = new SqlConnection(testString))
    {
        connection.Open();
        Console.WriteLine("Bağlantı başarılı! Veritabanına bağlanıldı."); // Başarılı mesaj

    }
}
catch (Exception ex)
{
    Console.WriteLine($"Bağlantı başarısız! Hata: {ex.Message}"); // Hata mesajını yazdır
}


#region HangFire
app.UseHangfireDashboard("/hangfire");

app.UseHangfireServer();
#endregion



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HandfireExample v1"));
}

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.Run();
