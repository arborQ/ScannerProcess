using QRCoder;

namespace CodeGenerator
{
    internal class QrCodeGenerator : ICodeGenerator
    {
        public void GenerateToFile(string message, string filePath)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (
                    var qrCodeData = qrGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new QRCode(qrCodeData))
                    {
                        using (var qrCodeImage = qrCode.GetGraphic(40))
                        {
                            qrCodeImage.Save(filePath);
                        }
                    }
                }
            }
        }
    }
}