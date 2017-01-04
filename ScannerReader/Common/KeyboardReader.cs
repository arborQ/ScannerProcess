using System.Text;
using System.Windows.Input;
using Common.Interfaces;

namespace Common
{
    public class KeyboardReader : IKeyboardReader
    {
        private readonly StringBuilder _scannerValue = new StringBuilder();


        public string NotifyChar(Key source)
        {
            if (source == Key.Return)
            {
                var outputString = _scannerValue.ToString().Trim();
                _scannerValue.Clear();
                return outputString;
            }

            var scannedChar = source.ToChar();

            if (scannedChar != ' ')
            {
                _scannerValue.Append(source.ToChar());
            }

            return null;
        }
    }
}