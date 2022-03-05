using Microsoft.AspNetCore.Mvc;

namespace Introduccion.NET.Ejercicio.Introduccion.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculadoraController : ControllerBase
    {
        
        private readonly ILogger<CalculadoraController> _logger;

        public CalculadoraController(ILogger<CalculadoraController> logger)
        {
            _logger = logger;
        }

        [HttpPost("operacion")]
        public int Post()
        {
            return 0;
        }

        [HttpGet("operacion/historial")]
        public int Get()
        {
            return 0;
        }
    }
}