using QRCoder;

namespace DynamicQR.QR_code.Services
{
	public static class QrGenerator
	{
		public static byte[] GenerateQrAsByte(string url)
		{
			QRCodeGenerator qrGenerator = new QRCodeGenerator();
			QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
			PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
			byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(5);

			return qrCodeAsPngByteArr;
		}
	}
}
