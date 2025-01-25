using DynamicQR.Common;
using DynamicQR.QR_code.DTOs;
using DynamicQR.QR_code.Services;
using QRCoder;
using System.Linq;
using static QRCoder.PayloadGenerator;

namespace DynamicQR.QR_code.Endpoint
{
	public class GetAllUserQRCodes : IEndpoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapGet("/getAllQr", Handle)
				.WithSummary("Get all user QR codes");
		}


		public record Response(string Title,string Type,string QrBase64,string Url);

		public static async Task<Results<Ok<List<Response>>,NotFound>> Handle(DynamicQrContext database, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
		{
			var rawData = database.Qrs
				.Join(database.TypeQrs,
				qr => qr.Type,
				type => type.Id,
				(qr, type) => new { qr.Title, qr.UserId, qr.Id, type.Type })
				.Where(x => x.UserId == claimsPrincipal.GetUserId())
				.ToList(); 

			string baseUrl = "google.com";
			Func<string, int?, int?, string> finalUrl = (baseUrl, userId, id) => $"{baseUrl}/{userId}/{id}";

			// Создаём список URL
			var urls = rawData.Select(x => finalUrl(baseUrl, x.UserId, x.Id)).ToList();

			// Генерация Base64 QR-кодов
			//var qrCodesBase64 = new List<string>();

			List<(string url, string qrBase64)> qrCodesBase64 = new List<(string, string)>();
			await Task.Run(() =>
			{
				Parallel.ForEach(urls, url =>
				{
					string imageBase64 = QrGenerator.GenerateQrAsBase64(url);
					lock (qrCodesBase64)
					{
						qrCodesBase64.Add((url,imageBase64));
					}
				});
			},cancellationToken);

			
			// Создаём результат, связывая URL и Base64
			var result = rawData
				.Select((x, index) => new Response(
					x.Title,
					x.Type,
					qrCodesBase64.ElementAtOrDefault(index).qrBase64,
					qrCodesBase64.ElementAtOrDefault(index).url
				))
				.ToList();


			return result.Count == 0
				? TypedResults.NotFound()
				: TypedResults.Ok(result);
		}
	}
}
