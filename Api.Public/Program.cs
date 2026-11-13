using Api.Public.Configs;
using Core.Authority.Configs;
using Core.History.Configs;
using Core.Identity.Configs;
using Core.Metadata.Configs;
using Core.Metadata.Options;
using Core.Monitor.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pack.Information.Configs;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMetadataCore(builder.Configuration);
builder.Services.AddHistoryCore(builder.Configuration.GetConnectionString("HistoryCS")!);
builder.Services.AddAuthorityCore(builder.Configuration.GetConnectionString("AuthorityCS")!);
builder.Services.AddIdentityCore(builder.Configuration.GetConnectionString("IdentityCS")!);
builder.Services.AddMonitorCore(builder.Configuration.GetConnectionString("MonitorCS")!);
builder.Services.AddScoped<IActiveUser, ActiveUser>();

builder.Services.AddInfoPackage(builder.Configuration.GetConnectionString("InfoCS")!);

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin", policy =>
	{
		policy.WithOrigins("http://localhost:3000")
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key không tồn tại.");

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,

		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

		ClockSkew = TimeSpan.Zero
	};
});

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	// Tạo endpoint JSON tại: /openapi/v1.json
	app.MapOpenApi();

	// Sử dụng Scalar làm giao diện UI
	app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ActiveUserMiddleware>();

app.MapControllers();

app.Run();
