using DynamicQR.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DynamicQR
{
	public static class ConfigureApp
	{
		public static async Task Configure(this WebApplication app)
		{
			app.UseHttpsRedirection();
			app.MapEndpoints();
		}


	}
}
