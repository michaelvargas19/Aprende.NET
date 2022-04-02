using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Entities
{
    public class AuditMessage
    {

        private readonly LogFormatterOptions _logFormatterOptions = new LogFormatterOptions();

        public AuditMessage(string className)
        {
            _logFormatterOptions.ClassName = className;
            LogMessage = new LogFormatter(_logFormatterOptions);
            StackTrace = System.Environment.StackTrace;
        }

        public LogFormatter LogMessage { get; }

        public string StepName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string User { get; set; }

        public string StackTrace { get; }

        public string AuditType { get; set; }
    }
}
