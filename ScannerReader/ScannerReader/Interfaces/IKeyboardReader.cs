using System.Windows.Input;

namespace Common.Interfaces
{
    public interface IKeyboardReader
    {
        string NotifyChar(Key source);
    }
}