using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Entity.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Repository.Contracts
{
    public interface ICalculatorRepository : IGenericRepository<Calculator, PracticaDBContext>
    {
    }
}
