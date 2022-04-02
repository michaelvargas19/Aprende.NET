using Introduccion.NET.Ejercicio.Business.Contracts;
using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using Introduccion.NET.Ejercicio.Entity.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Introduccion.NET.Ejercicio.API.Controllers.v1
{
    [Route("calculator/v1")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorProcess _calculatorProcess;

        public CalculatorController(ICalculatorProcess calculatorProcess)
        {
            this._calculatorProcess = calculatorProcess;
        }

        [HttpGet("enabled")]
        public async Task<IActionResult> GetEnabled()
        {
            var result = await _calculatorProcess.GetCalculatorsEnabled();
            return result.ToResponse();
        }
        
        [HttpPost()]
        public async Task<IActionResult> AddCalculator(CalculatorRequest request)
        {
            var result = await _calculatorProcess.AddCalculator(request);
            return result.ToResponse();
        }


    }
}
