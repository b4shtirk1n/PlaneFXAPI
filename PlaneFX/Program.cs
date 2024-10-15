using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using PlaneFX.Filters;
using PlaneFX.Middlewares;
using PlaneFX.Models;
using PlaneFX.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<PlaneFXContext>();

builder.Services.AddTransient<DbConnectionMiddleware>();

builder.Services.AddTransient<AuthFilter>();
builder.Services.AddTransient<AdminFilter>();

builder.Services.AddTransient<StartupService>();
builder.Services.AddTransient<AppService>();
builder.Services.AddTransient<AdminService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AccountService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<SubscribeService>();
builder.Services.AddTransient<CommandService>();

builder.Services.AddSwaggerGen(o =>
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
		o.WithOrigins("https://planefx.cloud/", "https://www.planefx.cloud/");
		o.AllowAnyHeader();
		o.AllowAnyMethod();
	});
}
app.UseMiddleware<DbConnectionMiddleware>();
app.MapControllers();
app.Run();

await app.Services.GetRequiredService<StartupService>().MakeSU();