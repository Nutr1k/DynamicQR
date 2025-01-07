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
		}

		private static void AddDatabase(this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<DynamicQrContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
			});
		}
	}
}
