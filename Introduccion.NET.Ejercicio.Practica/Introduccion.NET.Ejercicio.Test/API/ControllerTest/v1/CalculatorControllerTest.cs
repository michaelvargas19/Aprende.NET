using Introduccion.NET.Ejercicio.API.Controllers.v1;
using Introduccion.NET.Ejercicio.Business.Contracts;
using Introduccion.NET.Ejercicio.Common.Entities;
using Introduccion.NET.Ejercicio.Entity.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Test.API.ControllerTest.v1
{
    [TestClass]
    public class CalculatorControllerTest
    {
        private readonly Mock<ICalculatorProcess> _calculatorProcess = new Mock<ICalculatorProcess>();
        private CalculatorController _current;
        private ConfigurationBuilder _config;

        [TestInitialize]
        public void Inicializate()
        {
            _current = new CalculatorController(_calculatorProcess.Object);
            _config = new ConfigurationBuilder();
            _config.AddInMemoryCollection(new Dictionary<string, string>() {
                {"ConnectionString:PracticaDB", "Data Source=localhost;Initial Catalog=Practica;Integrated Security=True"}                
            });
        }


        [TestMethod]
        [DataRow(1, 200, DisplayName = "calculadora objects")]
        [DataRow(2, 200, DisplayName = "with out calculadora objects")]
        public async Task GetEnabledTest(int index, int expected)
        {
            //arrange
            if(index== 1)
            {
                _calculatorProcess.Setup(t => t.GetCalculatorsEnabled()).Returns(Task.FromResult(new ResponseBase<IEnumerable<CalculatorResponse>>()
                {
                    Code = 200,
                    Count = 1,
                    Data = new List<CalculatorResponse>()
                {
                    new CalculatorResponse()
                    {
                        Name = "Calculadora 1"
                    }
                }
                }));
            }

            if (index == 2)
            {
                _calculatorProcess.Setup(t => t.GetCalculatorsEnabled()).Returns(Task.FromResult(new ResponseBase<IEnumerable<CalculatorResponse>>()
                {
                    Code = 200,
                    Count = 0,
                    Data = new List<CalculatorResponse>() {}
                }));
            }

            //act
            IActionResult response = await _current.GetEnabled();

            //assert
            Assert.IsNotNull(response);

        }

    }

    
}
