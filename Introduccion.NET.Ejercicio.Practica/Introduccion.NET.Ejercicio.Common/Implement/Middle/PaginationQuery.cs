using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    /// <summary>
    /// Paginacion para los IQueriables de las consultas de los repos
    /// </summary>
    public static class PaginationQuery
    {

        /// <summary>
        /// Metodo encargado de paginar
        /// </summary>
        /// <typeparam name="T">Tipo de entidad a procesar</typeparam>
        /// <param name="query">Consulta que se pagina</param>
        /// <param name="page">Pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="orderBy">Metodo de ordenamiento</param>
        /// <param name="asList">true/false</param>
        /// <returns>Objeto con la paginacion de la consulta definida</returns>
        public static async Task<PagedResult<T>> Paginate<T>(this IQueryable<T> query, int page, int pageSize, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool asList = false) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = !asList ? query.Count() : query.ToList().Count
            };
            try
            {
                if (page == 0)
                {
                    result.Results = !asList ? await query.ToListAsync() : query.ToList();
                    return result;
                }

                if (!orderBy.IsEmpty()) query = orderBy(query);
                var pageCount = (double)result.RowCount / pageSize;
                result.PageCount = (int)Math.Ceiling(pageCount);

                var skip = (page - 1) * pageSize;
                var sql = query.Skip(skip).Take(pageSize).AsQueryable();
                result.Results = !asList ? await sql.ToListAsync() : sql.ToList();
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }

            return result;
        }
    }
}
