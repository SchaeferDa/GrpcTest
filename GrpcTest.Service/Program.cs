using Microsoft.OpenApi.Models;

namespace GrpcTest.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc().AddJsonTranscoding();

            builder.Services.AddGrpcSwagger();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "Math REST API", Version = "v1" });

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "GrpcTest.Shared.xml");
                c.IncludeXmlComments(filePath);
                c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<Services.MathService>();
            app.MapGrpcService<Services.SecurityService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpLogging();

            app.Run();
        }
    }
}