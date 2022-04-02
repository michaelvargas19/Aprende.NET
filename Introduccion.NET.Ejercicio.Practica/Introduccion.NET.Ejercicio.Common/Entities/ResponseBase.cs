using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Entities
{
    public class ResponseBase<T> : ICloneable
    {
        private Stopwatch _timer;

        public ResponseBase(bool startTimer = false)
        {
            Message = "";
            Code = (int)HttpStatusCode.OK;
            ProcessTimeSeg = 0;
            if (startTimer) StartTimer();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void StartTimer()
        {
            _timer = Stopwatch.StartNew();
        }

        public ResponseBase<T> StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                var num = 0m;
                var isDecimal = decimal.TryParse(_timer.Elapsed.TotalSeconds.ToString(), out num);
                num = isDecimal ? num : 0m;

                ProcessTimeSeg = num;
            }

            return this;
        }
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int Count { get; set; }
        public decimal ProcessTimeSeg { get; set; }
        public string MessageType { get; set; }
    }
}
