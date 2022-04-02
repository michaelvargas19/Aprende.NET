using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    /// <summary>
    /// Extensor dinamico para las entidades de los repositorios dinamicos.
    /// </summary>
    public static class RepositoriesExtension
    {

        private const string LASTDATE = "LastDate";
        private const string USERLOG = "UserLog";

        /// <summary>
        /// Metodo encargado de asignar valores a entidades en los repos dinamicos
        /// </summary>
        /// <typeparam name="T">Tipo de dato del objeto a procesar</typeparam>
        /// <param name="obj">Objeto a procesar</param>
        /// <returns>Objeto ya complementado</returns>
        public static T SetDefaultValues<T>(this T obj, IHttpContextAccessor _http) where T : class
        {
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add(LASTDATE, DateTime.Now.AddHours(-5));
            attributes.Add(USERLOG, _http.GetUserLog());

            if ((obj.GetType().GetProperty("Id") != null) && (obj.GetType().GetProperty("Id").GetValue(obj).ToString() == Guid.Empty.ToString()))
            {
                attributes.Add("Id", Guid.NewGuid());
            }

            (from p in typeof(T).GetProperties()
             join a in attributes on new { name = p.Name.ToLower() } equals new { name = a.Key.ToLower() }
             select new
             {
                 setProperty = p,
                 value = a.Value
             }).ToList().ForEach(y => { y.setProperty.SetValue(obj, y.value); });

            return obj;
        }

    }
}
