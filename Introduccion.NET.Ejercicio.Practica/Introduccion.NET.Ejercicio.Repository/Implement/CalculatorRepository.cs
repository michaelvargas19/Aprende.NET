using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using Introduccion.NET.Ejercicio.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Introduccion.NET.Ejercicio.Repository.Implement
{
    public class CalculatorRepository : GenericRepository<Calculator, PracticaDBContext>, ICalculatorRepository
    {
        public CalculatorRepository(IServiceScopeFactory serviceScope, IHttpContextAccessor http) : base(serviceScope, http)
        {

        }
    }
}
