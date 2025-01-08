using DynamicQR.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DynamicQR
{
	public static class ConfigureServices
	{
		public static void AddServices(this WebApplicationBuilder builder)
		{
			builder.AddDatabase();
			builder.AddSwagger();
			//Используется для регистрации валидаторов (классов валидации), которые наследуют AbstractValidator<T>
			//или реализуют IValidator<T>
			builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);
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
				builder.Services.AddSwaggerGen();
			}
		}
	}
}
