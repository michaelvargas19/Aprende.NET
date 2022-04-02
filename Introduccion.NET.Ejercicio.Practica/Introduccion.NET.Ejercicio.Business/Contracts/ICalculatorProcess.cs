using Introduccion.NET.Ejercicio.Common.Entities;
using Introduccion.NET.Ejercicio.Entity.DTO.Request;
using Introduccion.NET.Ejercicio.Entity.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Business.Contracts
{
    public interface ICalculatorProcess
    {
        Task<ResponseBase<IEnumerable<CalculatorResponse>>> GetCalculatorsEnabled();

        Task<ResponseBase<CalculatorResponse>> AddCalculator(CalculatorRequest request);

    }
}
