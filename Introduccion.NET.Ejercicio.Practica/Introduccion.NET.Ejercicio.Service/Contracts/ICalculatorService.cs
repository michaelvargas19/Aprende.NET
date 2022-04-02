using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using Introduccion.NET.Ejercicio.Repository;

namespace Introduccion.NET.Ejercicio.Service.Contracts
{
    public interface ICalculatorService : IGenericService<Calculator, PracticaDBContext>
    {

    }
}
