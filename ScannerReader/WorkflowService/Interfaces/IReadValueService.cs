using System;
using System.Threading.Tasks;

namespace WorkflowService.Interfaces
{
    public interface IReadValueService
    {
        bool ReadValue(int expected, DialogPosition position = DialogPosition.Center);

        void Wait(TimeSpan time);
    }

    public enum DialogPosition
    {
        Center = 0, Left = 1, Right = 2
    }
}