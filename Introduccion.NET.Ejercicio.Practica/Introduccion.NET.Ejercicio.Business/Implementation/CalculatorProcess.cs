using Introduccion.NET.Ejercicio.Business.Contracts;
using Introduccion.NET.Ejercicio.Common.Entities;
using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using Introduccion.NET.Ejercicio.Entity.DTO.Request;
using Introduccion.NET.Ejercicio.Entity.DTO.Response;
using Introduccion.NET.Ejercicio.Service.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Business.Implementation
{
    public class CalculatorProcess : ICalculatorProcess
    {

        private readonly ICalculatorService _calculatorService;
        
        public CalculatorProcess(ICalculatorService calculatorService
                              )
        {
            _calculatorService = calculatorService;
        }


        public async Task<ResponseBase<IEnumerable<CalculatorResponse>>> GetCalculatorsEnabled()
        {
            try
            {
                //Response
                ResponseBase<IEnumerable<CalculatorResponse>> response = new ResponseBase<IEnumerable<CalculatorResponse>>();

                //Process
                ResponseBase<PagedResult<Calculator>> resPack = await _calculatorService.ReadWithInclude(p => p.Status,p=> p.Include(a => a.Operations));
                List<CalculatorResponse> packageDTO = toResponse(resPack.Data.Results);

                //Close response
                GenericUtility.CloseRequest(response, packageDTO, "", 1, (int)HttpStatusCode.OK);

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return GenericUtility.ResponseBaseCatch<IEnumerable<CalculatorResponse>>(true, ex, HttpStatusCode.InternalServerError, null);
            }
        }

        public async Task<ResponseBase<CalculatorResponse>> AddCalculator(CalculatorRequest request)
        {
            try
            {
                ResponseBase<CalculatorResponse> response = new ResponseBase<CalculatorResponse>();
                Calculator calculator = new Calculator();

                calculator.Id = Guid.NewGuid();
                calculator.Name = request.Name;
                calculator.Status = true;

                ResponseBase<Calculator> resCal = await _calculatorService.Create(calculator);

                CalculatorResponse calculatorDTO = toResponse(resCal.Data);

                GenericUtility.CloseRequest(response, calculatorDTO, "", 1, (int)HttpStatusCode.OK);

                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return GenericUtility.ResponseBaseCatch<CalculatorResponse>(true, ex, HttpStatusCode.InternalServerError, null);
            }
        }



        #region Private

        private List<CalculatorResponse> toResponse(List<Calculator> entities)
        {
            List<CalculatorResponse> response = new List<CalculatorResponse>();

            if ((entities != null))
            {
                foreach (Calculator s in entities)
                {
                    response.Add(toResponse(s));
                }
            }

            return response;

        }

        private static CalculatorResponse toResponse(Calculator entity)
        {
            if (entity == null)
                return null;

            CalculatorResponse response = new CalculatorResponse();
            response.Id = entity.Id;
            response.Name = entity.Name;
            
            return response;

        }
        #endregion


    }
}
