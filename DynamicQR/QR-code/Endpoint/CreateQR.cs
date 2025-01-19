using DynamicQR.Authentication.Services;
using DynamicQR.Common;
using DynamicQR.Common.Api.Extension;
using DynamicQR.Data;
using DynamicQR.Data.Types;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DynamicQR.QR_code.Endpoint
{
	public class CreateQR : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/", Handle)
			   .WithSummary("Creates a new post")
			   .WithRequestValidation<Request>();
			   //.WithEnsureEntityExists<User, Request>(x => x.Id);

		}

		public record Request(string type,string title,string jsonVariables);

		public record Response(string url);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.type).NotEmpty();
				RuleFor(x => x.title).NotEmpty();
				RuleFor(x => x.jsonVariables).NotEmpty();
			}
		}


		private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(Request request, DynamicQrContext database, Jwt jwt, CancellationToken cancellationToken)
		{
			return TypedResults.Ok(new Response("test"));
		}
	}
}
