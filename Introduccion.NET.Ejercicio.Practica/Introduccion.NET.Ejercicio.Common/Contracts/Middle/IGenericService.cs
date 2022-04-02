using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Contracts.Middle
{
    public interface IGenericService<TEntity, TContext>
            where TEntity : class
            where TContext : DbContext
    {

        /// <summary>
        /// Metodo encargado de insertar un registro del Tipo T en BD
        /// </summary>
        /// <param name="entity">Entidad a procesar</param>
        /// <returns>Entidad procesada</returns>
        Task<ResponseBase<TEntity>> Create(TEntity entity);


        /// <summary>
        /// Lee todos los registros de la entidad
        /// </summary>
        /// <returns>Listado de registros de la entidad</returns>
        Task<ResponseBase<List<TEntity>>> ReadAll();

        /// <summary>
        /// Lee todos los registros de la entidad con paginación
        /// </summary>
        /// <param name="orderBy">Funcion que determina el ordenador para la paginación</param>
        /// <param name="page">Pagina</param>
        /// <param name="size">Tamaño de la pagina</param>
        /// <returns>Objeto con el resultado de la consulta</returns>
        Task<ResponseBase<PagedResult<TEntity>>> Read(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page = 0, int size = 0);

        /// <summary>
        /// Lee todos los registros con la capacidad de incluir entidades relacionadas en BD, incluyendo paginación
        /// </summary>
        /// <param name="expression">Expresion que representa el filtro de la entidad</param>
        /// <param name="include">Funcion que determina las entidades que se relacionan con la entidad</param>
        /// <param name="orderBy">Funcion que determina el ordenador para la paginación</param>
        /// <param name="page">Pagina</param>
        /// <param name="size">Tamaño de la pagina</param>
        /// <returns>Objeto con el resultado de la consulta</returns>
        Task<ResponseBase<PagedResult<TEntity>>> Read(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page = 0, int size = 0);

        /// <summary>
        /// Filtra y lista los registros de una entidad sin paginar.
        /// </summary>
        /// <param name="expression">Expresion que representa el filtro de la entidad</param>
        /// <returns>Listado de registros de la entidad</returns>
        Task<ResponseBase<List<TEntity>>> Read(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<ResponseBase<PagedResult<TEntity>>> ReadWithInclude(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);


        /// <summary>
        /// Metodo que actualiza la entidad con la informacion suministrada
        /// </summary>
        /// <param name="entity">Entidad procesada</param>
        /// <returns>True/False</returns>
        Task<ResponseBase<bool>> Update(TEntity newEntity, TEntity oldEntity);


        /// <summary>
        /// Metodo encargado de eliminar entidades a partir de un filtro
        /// </summary>
        /// <param name="expression">Expresion que representa el filtro de la entidad</param>
        /// <returns>true/false</returns>
        Task<ResponseBase<bool>> Delete(Expression<Func<TEntity, bool>> expression);
    }
}
