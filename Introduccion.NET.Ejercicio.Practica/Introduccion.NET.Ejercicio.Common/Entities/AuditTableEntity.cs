using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Entities
{
    public class AuditTableEntity
    {
        [DataMember(Name = "LastDate")]
        [IgnoreDataMember]
        public DateTime? LastDate { get; set; }

        [DataMember(Name = "UserLog")]
        [IgnoreDataMember]
        public string UserLog { get; set; }
    }

    public class AuditTableEntity<T>
    {
        [DataMember(Name = "LastDate")]
        [IgnoreDataMember]
        public DateTime? LastDate { get; set; }

        [DataMember(Name = "UserLog")]
        [IgnoreDataMember]
        public string UserLog { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public T GetClone { get; set; }

        public T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
