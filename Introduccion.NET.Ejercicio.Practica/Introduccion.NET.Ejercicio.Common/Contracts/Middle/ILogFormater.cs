using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Contracts.Middle
{
    public interface ILogFormatter
    {
        public string FormatMessage(LogType logType, string nameSpace, string details, [CallerMemberName] string caller = "");
    }


    public enum LogType
    {
        Debug,
        Information,
        Warning,
        Error
    }
}
