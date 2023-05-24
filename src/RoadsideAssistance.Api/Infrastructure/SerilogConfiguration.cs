using Serilog;

namespace RoadsideAssistance.Api.Infrastructure
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((context, services, config) => config.ReadFrom.Configuration(context.Configuration)
            .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message: lj} {NewLine} {Exception}"));
        }
    }
}
