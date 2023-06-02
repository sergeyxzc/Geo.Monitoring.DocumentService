using Geo.Monitoring.DocumentService;
using Geo.Monitoring.DocumentService.Application;
using Geo.Monitoring.DocumentService.Persistent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDocumentDbContext(new DocumentDbOptions()
{
    ConnectionString = "server=localhost;port=3306;uid=root;pwd=root;database=geotest"
});

builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.AddConsole();
});

builder.Services.AddScoped<DocumentDatabaseMigration>();
await DatabaseMigration.TryMigrateAsync(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
