using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using System.Runtime.CompilerServices;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class LogFormatter : ILogFormatter
    {

        string _path = string.Empty;
        string _process = string.Empty;
        string _namespace = string.Empty;
        string _detail = string.Empty;


        public string Path { get { return _path; } }
        public string Process { get { return _process; } }
        public string Namespace { get { return _namespace; } }
        public string Detail { get { return _detail; } }

        public LogFormatter(LogFormatterOptions options)
        {
            ClassName = options.ClassName;
        }

        public string ClassName { get; set; }

        public string FormatMessage(LogType logType, string nameSpace, string details, [CallerMemberName] string caller = "")
        {
            _path = $"{logType}-{nameSpace}-{ClassName}-{caller}";
            _process = caller;
            _namespace = nameSpace;
            _detail = details;
            return _path;
        }
    }

    public class LogFormatterOptions
    {
        public string ClassName { get; set; }
    }
}
