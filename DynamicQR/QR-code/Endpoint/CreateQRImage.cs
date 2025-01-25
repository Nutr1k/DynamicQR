using DynamicQR.Common;
using DynamicQR.Common.Api.Filters;
using DynamicQR.QR_code.Services;

namespace DynamicQR.QR_code.Endpoint
{
	public class CreateQRImage : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/CreateQrImage", Handle)
				.WithSummary("Get QR code images")
				.WithRequestValidation<Request>();
		}

		public record Request(string baseUrl, int UserId,int QrId);

		public class RequestValidation : AbstractValidator<Request>
		{
			public RequestValidation()
			{
				RuleFor(x => x.UserId).GreaterThan(0);
				RuleFor(x => x.QrId).GreaterThan(0);
			}
		}

		public record Response(string byteArr);
		public static Ok<Response> Handle(Request request, DynamicQrContext database)
		{
			string url = $"{request.baseUrl}/{request.UserId}/{request.QrId}";

			return TypedResults.Ok(new Response(QrGenerator.GenerateQrAsBase64(url)));
		}
	}
}
