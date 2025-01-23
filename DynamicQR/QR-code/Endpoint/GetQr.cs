using DynamicQR.Common;

namespace DynamicQR.QR_code.Endpoint
{
	public class GetQr : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/{UserId}/{QrId}", Handle)
				.WithSummary("Get a Qr code")
				.WithRequestValidation<Request>()
				.WithEnsureEntityExists<Qr, Request>(x => x.QrId);
		}

		public record Request(int UserId,int QrId);

		public record Response(int id);

		private static async Task<Ok> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			//var qr=database.Qrs.Where(x=>x.)
			return TypedResults.Ok();
		}
	}
}
