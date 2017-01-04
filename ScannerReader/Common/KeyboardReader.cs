using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Common.Interfaces;

namespace Common
{
    public class KeyboardReader : IKeyboardReader
    {
        private readonly List<char> _notifiedChars;

        public KeyboardReader(List<char> notifiedChars)
        {
            _notifiedChars = notifiedChars;
        }

        public string NotifyChar(Key source)
        {
            if (source == Key.Return)
            {
                var sb = new StringBuilder();
                _notifiedChars.ForEach(c => sb.Append(c));
                _notifiedChars.Clear();
                return sb.ToString();
            }

            _notifiedChars.Add(source.ToChar());
            return null;
        }
    }
}