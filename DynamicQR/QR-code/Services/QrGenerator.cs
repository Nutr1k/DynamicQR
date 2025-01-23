using QRCoder;

namespace DynamicQR.QR_code.Services
{
	public static class QrGenerator
	{
		public static string GenerateQrAsBase64(string url)
		{
			QRCodeGenerator qrGenerator = new QRCodeGenerator();
			QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
			Base64QRCode qrCode = new Base64QRCode(qrCodeData);
			string qrCodeImageAsBase64 = qrCode.GetGraphic(5);
			return qrCodeImageAsBase64;
		}
	}
}
