using DynamicQR.Authentication.Services;
using DynamicQR.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DynamicQR
{
	public static class ConfigureServices
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.AddDatabase();
			builder.AddSwagger();


			

			builder.AddJwtAuthentication();

			builder.AddFluentValidation();
		}

		private static void AddFluentValidation(this WebApplicationBuilder builder)
		{
			//Используется для регистрации валидаторов (классов валидации), которые наследуют AbstractValidator<T>
			//или реализуют IValidator<T>(FluentValidation)
			builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);

			ValidatorOptions.Global.LanguageManager.Enabled = false;
		}

		//Для dependency injection(чтобы получать доступ к бд из конструкторов, методов)
		private static void AddDatabase(this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<DynamicQrContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
		}

		private static void AddSwagger(this WebApplicationBuilder builder)
		{
			if (builder.Environment.IsDevelopment())
			{
				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen(options =>
				{
					options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
					options.InferSecuritySchemes();
				});
			}
		}
		private static void AddJwtAuthentication(this WebApplicationBuilder builder)
		{
			builder.Services.AddAuthentication().AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = Jwt.SecurityKey(builder.Configuration["Jwt:Key"]!),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero,
					RoleClaimType = ClaimTypes.Role
				};
			});
			builder.Services.AddAuthorization();

			builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
			builder.Services.AddTransient<Jwt>();//DI
		}
	}
}
