using QRCoder;

namespace CodeGenerator
{
    internal class QrCodeGenerator : ICodeGenerator
    {
        private readonly ICodeSerializer _codeSerializer;

        public QrCodeGenerator(ICodeSerializer codeSerializer)
        {
            _codeSerializer = codeSerializer;
        }

        public void GenerateToFile(string message, string filePath)
        {
            var serializedCode = _codeSerializer.SerializeCode(message + '\r');

            using (var qrGenerator = new QRCodeGenerator())
            {
                using (
                    var qrCodeData = qrGenerator.CreateQrCode(serializedCode, QRCodeGenerator.ECCLevel.Q))
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