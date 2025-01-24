using DynamicQR.Authentication.Endpoints;
using DynamicQR.Common;
using DynamicQR.File.Endpoint;
using DynamicQR.QR_code;
using DynamicQR.QR_code.Endpoint;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace DynamicQR
{
	public static class Endpoints
	{
		private static readonly OpenApiSecurityScheme securityScheme = new()
		{
			Type = SecuritySchemeType.Http,
			Name = JwtBearerDefaults.AuthenticationScheme,
			Scheme = JwtBearerDefaults.AuthenticationScheme,
			Reference = new()
			{
				Type = ReferenceType.SecurityScheme,
				Id = JwtBearerDefaults.AuthenticationScheme
			}
		};
		public static void MapEndpoints(this WebApplication app)
		{
			var endpoints = app;//.MapGroup("");
			//.AddEndpointFilter<RequestLoggingFilter>() //В последующем настроим для логирования
			//.WithOpenApi();

			endpoints.MapAuthenticationEndpoints();//Шаг #2. Конфигурация конечных точек.
			endpoints.MapQrCodeEndpoints();
			endpoints.MapFileEndpoints();
			
		}
		private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
		{
			var endpoints = app.MapGroup("/auth")
				.WithTags("Authentication");//Для метаданных

			endpoints.MapPublicGroup()//Шаг #3. Конфигурация конечных точек.
				.MapEndpoint<Login>()
				.MapEndpoint<Signup>();
		}

		private static void MapQrCodeEndpoints(this IEndpointRouteBuilder app)
		{
			var endpoints = app.MapGroup("/qr")
				.WithTags("QR operations");
			
			endpoints.MapPublicGroup()
				.MapEndpoint<GetQRImage>();


			endpoints.MapAuthorizedGroup()
				.MapEndpoint<GetQrTypes>()
				.MapEndpoint<GetTemplateSchema>()
				.MapEndpoint<CreateQR>()
				.MapEndpoint<DeleteQR>()
				.MapEndpoint<UpdateQR>();
			//endpoints.MapAdminGroup()
			//.MapEndpoint<GetQrTypes>();

		}


		private static void MapFileEndpoints(this IEndpointRouteBuilder app)
		{
			var endpoints = app.MapGroup("/file")
				.WithTags("Files");//Для метаданных

			endpoints.MapAuthorizedGroup()//Шаг #3. Конфигурация конечных точек.
				.MapEndpoint<UploadFile>();
		}

		#region Пояснение
		/*
		Создание перфикса для маршрута
		В нашем случае он пустой, но оставляем для дальнейшего расширения
		Пример
		endpoints.MapPublicGroup("admin")
				.MapEndpoint<Login>();
		URL в данном случае была бы такой auth/admin/login
		*/
		#endregion
		private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
		{
			return app.MapGroup(prefix ?? string.Empty)
				.AllowAnonymous();
		}

		private static RouteGroupBuilder MapAuthorizedGroup(this IEndpointRouteBuilder app, string? prefix = null)
		{
			return app.MapGroup(prefix ?? string.Empty)
				.RequireAuthorization()//Добавляет политику авторизации по умолчанию в конечные точки
				.WithOpenApi(x => new(x)
				{
					Security = [new() { [securityScheme] = [] }],
				});
		}

		private static RouteGroupBuilder MapAdminGroup(this IEndpointRouteBuilder app, string? prefix = null)
		{
			return app.MapGroup(prefix ?? string.Empty)
				.RequireAuthorization(policy => policy.RequireRole("Admin"))//Добавляет политику авторизации по роли
				.WithOpenApi(x => new(x)
				{
					Security = [new() { [securityScheme] = [] }],
				});
		}

		#region Установка маршрута конечной точки

		#endregion
		private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
		{
			TEndpoint.Map(app);//Шаг #4. Вызываем релизацию метода Map конркетной конченой точки
			return app;//app — это параметр типа IEndpointRouteBuilder, который представляет объект, используемый для конфигурации маршрутов (эндпоинтов) в приложении.
		}
	}
}
