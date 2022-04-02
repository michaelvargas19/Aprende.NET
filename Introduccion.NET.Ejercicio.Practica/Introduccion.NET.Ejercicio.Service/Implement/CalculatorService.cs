using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Implement.Middle;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using Introduccion.NET.Ejercicio.Repository;
using Introduccion.NET.Ejercicio.Repository.Contracts;
using Introduccion.NET.Ejercicio.Service.Contracts;

namespace Introduccion.NET.Ejercicio.Service.Implement
{
    public class CalculatorService : GenericService<Calculator, PracticaDBContext>, ICalculatorService
    {
        public CalculatorService(ICalculatorRepository repository, IAuditManager auditManager) : base(repository, null, auditManager)
        {

        }
    }
}
