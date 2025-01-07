using DynamicQR.Authentication.Endpoints;
using DynamicQR.Common;

namespace DynamicQR
{
	public static class Endpoints
	{
		private static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
		{
			var endpoints = app.MapGroup("/auth")
				.WithTags("Authentication");

			endpoints.MapPublicGroup()
				.MapEndpoint<Login>();
		}
		private static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder app, string? prefix = null)
		{
			return app.MapGroup(prefix ?? string.Empty)
				.AllowAnonymous();
		}
		private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
		{
			TEndpoint.Map(app);
			return app;
		}

		public static void MapEndpoints(this WebApplication app)
		{
			var endpoints = app.MapGroup("")
				//.AddEndpointFilter<RequestLoggingFilter>()
				.WithOpenApi();

			endpoints.MapAuthenticationEndpoints();
		}
	}
}
