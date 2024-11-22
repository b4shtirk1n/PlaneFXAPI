using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PlaneFX.Extensions;
using PlaneFX.Middlewares;
using PlaneFX.Models;
using PlaneFX.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using StackExchange.Redis;

namespace PlaneFX
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var services = builder.Services;
			var configuration = builder.Configuration;

			services.AddSerilog((logger) => logger
				.MinimumLevel.Information()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
				.WriteTo.Console()
				.WriteTo.GrafanaLoki(configuration.GetConnectionString("Loki")
					?? throw new NullReferenceException()));

			services.AddControllers().AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				o.JsonSerializerOptions.WriteIndented = true;
			});
			services.AddEndpointsApiExplorer();

			services.AddHttpClient();
			services.AddServices();
			services.AddDbContext<PlaneFXContext>();
			services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
				.Connect(configuration.GetConnectionString("Redis")
					?? throw new NullReferenceException("Redis url not init")));


			services.AddSwaggerGen(o =>
			{
				o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
				{
					Name = nameof(Authorization),
					Type = SecuritySchemeType.ApiKey,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = nameof(System.IdentityModel.Tokens.Jwt).ToUpper(),
					In = ParameterLocation.Header,
					Description = "JSON Web Token based security (\"Bearer {Token}\")",
				});
				o.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = JwtBearerDefaults.AuthenticationScheme,
								Type = ReferenceType.SecurityScheme
							}
						},
						Array.Empty<string>()
					}
				});
			});

			var app = builder.Build();

			app.UseSerilogRequestLogging();
			app.UseSwagger();
			app.UseSwaggerUI();

			if (app.Environment.IsDevelopment())
			{
				app.UseCors(o =>
				{
					o.AllowAnyOrigin();
					o.AllowAnyHeader();
					o.AllowAnyMethod();
				});
			}
			else
			{
				app.UseCors(o =>
				{
					o.WithOrigins(
						"https://planefx.cloud/",
						"https://www.planefx.cloud/"
					);
					o.AllowAnyHeader();
					o.AllowAnyMethod();
					o.AllowCredentials();
				});
			}
			app.UseMiddleware<DbConnectionMiddleware>();
			app.MapControllers();

			await app.RunAsync();

			await app.Services.GetRequiredService<StartupService>().MakeSU();
		}
	}
}