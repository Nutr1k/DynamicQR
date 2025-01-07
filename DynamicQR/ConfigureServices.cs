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
	}
}
