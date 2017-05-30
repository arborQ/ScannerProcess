using System;

namespace ControllerService
{
    public class ControllerEvents
    {
        public Action<string> ChangeState { get; set; }

        public Action WorkDone { get; set; }

        public Action<Exception> Error { get; set; }
    }
}
