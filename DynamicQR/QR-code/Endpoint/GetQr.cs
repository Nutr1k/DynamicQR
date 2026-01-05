using DynamicQR.Common;

namespace DynamicQR.QR_code.Endpoint
{
	public class GetQr : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapPost("/test", Handle)
				.WithSummary("Get a Qr code")
				.WithRequestValidation<Request>()
				.WithEnsureEntityExists<Qr, Request>(x => x.QrId);
		}

		public record Request(int UserId,int QrId);

		public record Response(string title,string jsonTemplateSchema, string JsonVariables);

		private static async Task<Ok<Response>> Handle(Request request, DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{


			var result = await database.Qrs
				.Where(t => t.Id == request.QrId)
				.Select(t => new { t.Type, t.JsonVariables,t.Title })
				.SingleAsync(cancellationToken);

			
			int typeId = result.Type;
			

			var scheme = await database.TypeQrs.Where(x => x.Id == typeId).Select(x => x.JsonTemplateSchema).SingleAsync(cancellationToken);



			return TypedResults.Ok(new Response(result.Title,scheme,result.JsonVariables));
		}
	}
}
