using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace Logger.Renderers
{
    [LayoutRenderer("login-renderer")]
    public class LoginRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            throw new NotImplementedException();
        }
    }
}