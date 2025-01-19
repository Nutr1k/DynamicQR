using DynamicQR.Common;
using DynamicQR.Common.Api.Extension;
using DynamicQR.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DynamicQR.QR_code.Endpoint
{
	public class GetTemplateSchema : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app
			  .MapPost("/qrtemplate", Handle)
			  .WithSummary("Get all QR type")
			  .WithRequestValidation<Request>();

		}
		
		public record Request(int Id);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.Id).GreaterThan(0);
			}
		}

		public record Response(string type,string JsonTemplateSchema);

		private static async Task<Results<Ok<Response>, NotFound>> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var post = await database.TypeQrs
				.Select(t => new Response
				(
					t.Type,
					t.JsonTemplateSchema
				))
				.SingleOrDefaultAsync(cancellationToken);

			return post is null
				? TypedResults.NotFound()
				: TypedResults.Ok(post);
		}
	}
}
