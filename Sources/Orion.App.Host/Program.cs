using Orion.App.Dal.PostgreSQL.Infrastructure;
using Orion.App.Integration.SiriusProvider.Infrastructure;
using Orion.App.Integration.Hangfire.Infrastructure;
using Orion.App.Integration.Hangfire.Jobs;
using Venus.Shared.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DomainEventPublisher>();
builder.Services.ConfigureDataProvider(builder.Configuration);
builder.Services.ConfigurePostgreSQL(builder.Configuration);
builder.Services.ConfigureHangfire(builder.Configuration);
//builder.Services.ConfigureMongoDb(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddRecurringJob<LoadDataJob>(LoadDataJob.SettingsName);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();