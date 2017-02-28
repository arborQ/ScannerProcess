using System.Text;
using System.Windows.Input;
using Common.Interfaces;
using System.Timers;
using System;

namespace Common
{
    public class KeyboardReader : IKeyboardReader
    {
        private readonly StringBuilder _scannerValue = new StringBuilder();
        private readonly Timer _timer;

        public KeyboardReader()
        {
            _timer = new Timer(TimeSpan.FromMilliseconds(100).TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _scannerValue.Clear();
        }
            
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

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}