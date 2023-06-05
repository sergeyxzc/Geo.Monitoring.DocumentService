using Geo.Monitoring.DocumentService;
using Geo.Monitoring.DocumentService.Application;
using Geo.Monitoring.DocumentService.Auth;
using Geo.Monitoring.DocumentService.Persistent;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetSection("Database").Get<DocumentDbOptions>()!;
var swaggerOptions = builder.Configuration.GetSection("Swagger").Get<SwaggerOptions>()!;

// Add services to the container.
builder.Services.AddDocumentDbContext(dbOptions);

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddAuthSwagger();
});

builder.Services.AddLogging(loggerBuilder =>
{
    loggerBuilder.AddConsole();
});

await DatabaseMigration.TryMigrateAsync(builder.Services, dbOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (swaggerOptions.Enabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
