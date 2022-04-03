using Introduccion.NET.Ejercicio.Business.Contracts;
using Introduccion.NET.Ejercicio.Business.Implementation;
using Introduccion.NET.Ejercicio.Common.Entities;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using Introduccion.NET.Ejercicio.Service.Contracts;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Test.BusinessTest.ImplementionTest
{
    [TestClass]
    public class ICalculatorProcessTest
    {
        private readonly Mock<ICalculatorService> _calculatorService = new Mock<ICalculatorService>();
        private ICalculatorProcess _current;
        

        [TestInitialize]
        public void Inicializate()
        {
            _current = new CalculatorProcess(_calculatorService.Object);
        }
        

        [TestMethod]
        [DataRow(1, 200, DisplayName = "calculadora objects")]
        [DataRow(2, 200, DisplayName = "with out calculadora objects")]
        public async Task GetCalculatorsEnabled(int index, int expected)
        {
            //arrange
            if (index == 1)
            {
                _calculatorService
                    .Setup(a => a.ReadWithInclude(
                        It.IsAny<Expression<Func<Calculator, bool>>>(),
                        It.IsAny<Func<IQueryable<Calculator>, IIncludableQueryable<Calculator, object>>>()))
                        .Returns(Task.FromResult(new ResponseBase<PagedResult<Calculator>>()
                        {
                            Code = 200,
                            Count = 1,
                            Data = new PagedResult<Calculator>
                            {
                                Results = new List<Calculator> 
                                {
                                    new Calculator
                                    {
                                        Name = "Calculadora 1"
                                    }
                                }
                            }
                        }
                    ));
            }

            if (index == 2)
            {
                _calculatorService
                    .Setup(a => a.ReadWithInclude(
                        It.IsAny<Expression<Func<Calculator, bool>>>(),
                        It.IsAny<Func<IQueryable<Calculator>, IIncludableQueryable<Calculator, object>>>()))
                        .Returns(Task.FromResult(new ResponseBase<PagedResult<Calculator>>()
                        {
                            Code = 200,
                            Count = 0,
                            Data = new PagedResult<Calculator>
                            {
                                Results = new List<Calculator> { }
                            }
                        }
                    ));
            }

            //act
            var result = await _current.GetCalculatorsEnabled();

            //assert
            Assert.AreEqual(expected, result.Code);

            
            

        }
    }
}
