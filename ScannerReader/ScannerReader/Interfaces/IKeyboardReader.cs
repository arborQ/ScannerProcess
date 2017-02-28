using System;
using System.Windows.Input;

namespace Common.Interfaces
{
    public interface IKeyboardReader : IDisposable
    {
        string NotifyChar(Key source);
    }
}