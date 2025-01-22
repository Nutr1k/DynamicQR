using DynamicQR.Common.Api.Extension;
using DynamicQR.Data.Types;
using DynamicQR.Data;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DynamicQR.Common;

namespace DynamicQR.QR_code.Endpoint
{
	public class DeleteQR:IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/Delete", Handle)
			   .WithSummary("Deletes a QR")
			   .WithRequestValidation<Request>()
			   .WithEnsureUserOwnsEntity<Qr, Request>(x => x.QrId);

		}

		public record Request(int QrId);

		public record Response(string url);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.QrId).GreaterThan(0);
			}
		}


		private static async Task<Results<Ok, NotFound>> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var qrDeleted = await database.Qrs.Where(x => x.Id == request.QrId).ExecuteDeleteAsync(cancellationToken);


			return qrDeleted == 1
				? TypedResults.Ok()
				: TypedResults.NotFound();
		}
	}
}
