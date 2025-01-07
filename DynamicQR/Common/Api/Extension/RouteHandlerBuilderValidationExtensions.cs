using DynamicQR.Common.Api.Filters;

namespace DynamicQR.Common.Api.Extension
{
	public static class RouteHandlerBuilderValidationExtensions
	{
		public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
		{
			return builder
				.AddEndpointFilter<RequestValidationFilter<TRequest>>()
				.ProducesValidationProblem();
		}
	}
}
