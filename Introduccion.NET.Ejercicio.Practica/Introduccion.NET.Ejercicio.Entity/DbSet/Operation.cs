using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Entity.DbSet
{
    [Table("Operation")]
    public class Operation
    {
        public Guid Id { get; set; }

        [ForeignKey("Calculator")]
        public Guid IdCalculator { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        
        public Calculator Calculator { get; set; }

    }
}
