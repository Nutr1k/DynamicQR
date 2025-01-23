using DynamicQR.Common;
using DynamicQR.Common.Api.Filters;
using DynamicQR.QR_code.Services;

namespace DynamicQR.QR_code.Endpoint
{
	public class GetQRImage : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapGet("/{UserId}/{QrId}", Handle)
				.WithSummary("get QR code images")
				.WithRequestValidation<Request>();
		}

		public record Request(int UserId,int QrId);

		public class RequestValidation : AbstractValidator<Request>
		{
			public RequestValidation()
			{
				RuleFor(x => x.UserId).GreaterThan(0);
				RuleFor(x => x.QrId).GreaterThan(0);
			}
		}

		public record Response(string byteArr);
		public static Ok<Response> Handle([AsParameters]Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			string url = $"https://localhost:7205/{request.UserId}/{request.QrId}";

			return TypedResults.Ok(new Response(QrGenerator.GenerateQrAsBase64(url)));
		}
	}
}
