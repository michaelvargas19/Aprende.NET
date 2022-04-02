using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    /// <summary>
    /// Extensores para los registros para el application insigth
    /// </summary>
    public static class TelemetryException
    {
        public static void TrackAuditException(this TelemetryClient client, Exception e)
        {
            if (client != null)
            {
                try
                {
                    client.TrackEvent("API-Exception", e.ToDictionary());
                }
                catch (Exception ex)
                {
                    Console.Write($"Error Telemetry: {ex}");
                    Console.Write($"Error Trace: {e}");
                }
            }
        }

        private static Dictionary<string, string> ToDictionary(this Exception e)
        {
            var st = new StackTrace(e, true);
            var frame = st.GetFrame(0);
            var line = frame == null ? 0 : frame.GetFileLineNumber();

            var dic = new Dictionary<string, string>
            {
                {"Message", e.Message},
                {"Line", line.ToString()},
                {"StackTrace", e.StackTrace },
                {"InnerException", e.InnerException?.Message },
                {"ToString", e.ToString() }
            };

            return dic;
        }
    }
}
