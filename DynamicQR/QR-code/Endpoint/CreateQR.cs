using Chirper.Data.Types;
using DynamicQR.Authentication.Services;
using DynamicQR.Common;
using DynamicQR.Common.Api.Extension;
using DynamicQR.Data;
using DynamicQR.Data.Types;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DynamicQR.QR_code.Endpoint
{
	public class CreateQR : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/", Handle)
			   .WithSummary("Creates a new QR")
			   .WithRequestValidation<Request>()
			   .WithEnsureEntityExists<TypeQr, Request>(x => x.typeId);

		}

		public record Request(int typeId,string title,string jsonVariables);

		public record Response(string url);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.typeId).NotEmpty();
				RuleFor(x => x.title).NotEmpty();
				RuleFor(x => x.jsonVariables).NotEmpty();
			}
		}


		private static async Task<Ok<Response>> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal,  CancellationToken cancellationToken)
		{
			var Qr = new Qr
			{
				UserId = claimsPrincipal.GetUserId(),
				JsonVariables=request.jsonVariables,
				Title=request.title,
				Type=request.typeId,
			};
			await database.Qrs.AddAsync(Qr,cancellationToken);
			await database.SaveChangesAsync(cancellationToken);

			return TypedResults.Ok(new Response($"{claimsPrincipal.GetUserId()}/{Qr.Id}"));
		}
	}
}
