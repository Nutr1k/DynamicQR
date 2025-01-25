global using DynamicQR.Common.Api;
global using DynamicQR.Common.Api.Extensions;
global using DynamicQR.Common.Api.Results;
global using DynamicQR.Data;
global using DynamicQR.Data.Types;
global using FluentValidation;
global using Microsoft.AspNetCore.Http.HttpResults;
global using Microsoft.EntityFrameworkCore;
global using System.Security.Claims;

namespace DynamicQR
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			builder.AddServices();
			// Для Razor страниц
			builder.Services.AddRazorPages();


			WebApplication app = builder.Build();

			await app.Configure();

			#region Для UI(Web)
			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.MapRazorPages();
			#endregion
			
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.Run();
		}
	}
}
