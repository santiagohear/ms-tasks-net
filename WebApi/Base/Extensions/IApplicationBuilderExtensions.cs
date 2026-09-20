using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;

namespace WebApi.Base.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder application)
        {
            return application.UseCors("AllowAll");
        }

        public static IApplicationBuilder UseHeaders(this IApplicationBuilder application)
        {
            application.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "master-only");
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Append("Cache-Control", "no-cache,no-store,must-revalidate");
                context.Response.Headers.Append("Pragma", "no-cache");
                context.Response.Headers.Remove("X-Powered-By");
                context.Response.Headers.Remove("Server");
                await next();
            });

            return application;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder application, IConfiguration configuration)
        {
            var swaggerRoutePrefix = string.IsNullOrWhiteSpace(configuration["PathBase"])
                ? "swagger"
                : $"{configuration["PathBase"]!.TrimStart('/')}-swagger";

            application.UseSwagger(c =>
            {
                c.RouteTemplate = $"{swaggerRoutePrefix}/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    var pathBase = configuration.GetValue<string>("PathBase") ?? string.Empty;
                    swagger.Servers = new[] { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{pathBase}" } };
                });
            });

            application.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint($"/{swaggerRoutePrefix}/v1/swagger.json", "Tasks.WebApi.V1");
                setupAction.RoutePrefix = $"{swaggerRoutePrefix}";
            });

            return application;
        }
    }
}
