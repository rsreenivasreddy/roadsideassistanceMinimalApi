using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadsideAssistance.Api.Data;
using RoadsideAssistance.Api.Filters;
using RoadsideAssistance.Api.Infrastructure;
using RoadsideAssistance.Api.Models.Request;
using RoadsideAssistance.Api.Models.Response;
using RoadsideAssistance.Api.Services;
using RoadsideAssistance.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();
//builder.Services.AddProblemDetails()
//    .AddProblemDetailsConventions();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));
builder.Services.AddScoped<ICustomerAssistantService, CustomerAssistantService>();
builder.Services.AddScoped<IRoadsideAssistanceService, RoadsideAssistanceService>();
builder.Services.AddScoped<IValidator<GeoLocationRequest>, GeoLocationRequestValidator>();
builder.Services.AddDbContext<DataContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DataContext"),
                  builder =>
                  {
                      builder.EnableRetryOnFailure(3);
                      builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                  }));

builder.Services.AddHealthChecks()
   .AddDbContextCheck<DataContext>();
  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPut("v1/Assistants/{id}/Location", ([FromServices] ICustomerAssistantService customerAssistantService, GeoLocationRequest geoLocationRequest, int id, CancellationToken cancellationToken) =>
{
    return customerAssistantService.UpdateAssistantLocation(id, geoLocationRequest, cancellationToken);
})
.AddEndpointFilter<GeoLocationRequestFilter>()
.WithTags("Assistant")
.WithName("UpdateAssistantLocation")
.WithOpenApi();

app.MapPost("v1/Assistants/FindNearest", ([FromServices] ICustomerAssistantService customerAssistantService,GeoLocationRequest geoLocationRequest, int limit, CancellationToken cancellationToken) =>
{
    return customerAssistantService.FindNearestAssistants(geoLocationRequest, limit,cancellationToken);
})
.AddEndpointFilter<GeoLocationRequestFilter>()
.Produces<List<AssistantResponse>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.WithTags("Assistant")
.WithName("GetNearestAssistants")
.WithOpenApi();

app.MapPost("v1/Customers/{id}/ReserveAssistant", ([FromServices] ICustomerAssistantService customerAssistantService, GeoLocationRequest geoLocationRequest, int id, CancellationToken cancellationToken) =>
{
    return customerAssistantService.ReserveAssistant(id, geoLocationRequest,cancellationToken);
})
.AddEndpointFilter<GeoLocationRequestFilter>()
.Produces<AssistantResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.WithTags("Customer")
.WithName("ReserveAssistance")
.WithOpenApi();

app.MapPut("v1/Customers/{id}/Assistants/{assistantId}/Release", ([FromServices] ICustomerAssistantService customerAssistantService, int id, int assistantId, CancellationToken cancellationToken) =>
{
    return customerAssistantService.ReleaseAssistant(id, assistantId, cancellationToken);
})
.WithTags("Customer")
.WithName("ReleaseAssistance")
.WithOpenApi();

app.MapHealthChecks("/health/ready", new HealthCheckOptions()).WithName("HealthCheck-Ready");
app.MapHealthChecks("/health/live", new HealthCheckOptions()).WithName("HealthCheck-Live") ;
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
