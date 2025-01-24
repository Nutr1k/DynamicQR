using DynamicQR.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DynamicQR
{
	public static class ConfigureApp
	{
		public static async Task Configure(this WebApplication app)
		{
			app.UseHttpsRedirection();//Для перенаправления всех HTTP-запросов на HTTPS.
			app.MapEndpoints();//Шаг #1. Конфигурация конечных точек.
			
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
					options.RoutePrefix = string.Empty;
				});
			}
		}


	}
}
