using System;
using System.Threading.Tasks;

namespace WorkflowService.Interfaces
{
    public interface IReadValueService
    {
        bool ReadValue(int expected);

        void Wait(TimeSpan time);
    }
}