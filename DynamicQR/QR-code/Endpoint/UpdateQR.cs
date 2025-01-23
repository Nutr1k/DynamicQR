using DynamicQR.Common;
namespace DynamicQR.QR_code.Endpoint
{
	public class UpdateQR : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPut("/update", Handle)
				.WithSummary("Update a QR")
				.WithRequestValidation<Request>()
				.WithEnsureUserOwnsEntity<Qr, Request>(x => x.id);
		}
		public record Request(int id, string title, string jsonVariables);

		public class RequestValidator : AbstractValidator<Request>
		{
			public RequestValidator()
			{
				RuleFor(x => x.id).GreaterThan(0);
				RuleFor(x => x.title)
					.NotEmpty()
					.MaximumLength(50);
			}
		}


		private static async Task<Ok> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var qr = await database.Qrs.SingleAsync(x => x.Id == request.id, cancellationToken);
			qr.Title = request.title;
			qr.JsonVariables = request.jsonVariables;

			await database.SaveChangesAsync(cancellationToken);

			return TypedResults.Ok();
		}
	}

}
