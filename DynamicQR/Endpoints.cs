using DynamicQR.Authentication.Endpoints;
using DynamicQR.Common;

namespace DynamicQR
{
	public static class Endpoints
	{
		private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
		{
			var endpoints = app.MapGroup("/auth")
				.WithTags("Authentication");//Для метаданных

			endpoints.MapPublicGroup()//Шаг #3. Конфигурация конечных точек.
				.MapEndpoint<Login>()
				.MapEndpoint<Signup>();
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

		#region Установка маршрута конечной точки

		#endregion
		private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
		{
			TEndpoint.Map(app);//Шаг #4. Вызываем релизацию метода Map конркетной конченой точки
			return app;//app — это параметр типа IEndpointRouteBuilder, который представляет объект, используемый для конфигурации маршрутов (эндпоинтов) в приложении.
		}

		public static void MapEndpoints(this WebApplication app)
		{
			var endpoints = app.MapGroup("");
				//.AddEndpointFilter<RequestLoggingFilter>() //В последующем настроим для логирования
				//.WithOpenApi();

			endpoints.MapAuthenticationEndpoints();//Шаг #2. Конфигурация конечных точек.
		}
	}
}
