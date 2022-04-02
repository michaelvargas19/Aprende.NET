using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.ApplicationInsights;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    /// <summary>
    /// Servicio de negocio Generico.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericService<TEntity, TContext> : IGenericService<TEntity, TContext>
                where TEntity : class
                where TContext : DbContext
    {
        private readonly IGenericRepository<TEntity, TContext> _repository;
        private readonly TelemetryClient _log;
        private readonly IAuditManager _auditManager;

        /// <summary>
        /// Inicializa el repositorio y el log de telemetria para el aplicationinsigth
        /// </summary>
        /// <param name="repository"></param>
        public GenericService(IGenericRepository<TEntity, TContext> repository, TelemetryClient log, IAuditManager auditManager)
        {
            _repository = repository;
            _log = log;
            _auditManager = auditManager;
        }

        public async Task<ResponseBase<TEntity>> Create(TEntity entity)
        {
            AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
            ResponseBase<TEntity> response = new ResponseBase<TEntity>();
            try
            {
                response.Data = await _repository.Create(entity).ConfigureAwait(true);
                // Audit
                auditMessage.NewValue = entity.GetSerializedEntity();
                auditMessage.LogMessage.FormatMessage(LogType.Information, $"{this.GetType().Namespace}.{this.GetType().Name}", "");
                await _auditManager.SaveAuditAsync(auditMessage);
                return response;
            }
            catch (Exception ex)
            {
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<TEntity>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }

        public async Task<ResponseBase<bool>> Delete(Expression<Func<TEntity, bool>> expression)
        {
            AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
            ResponseBase<bool> response = new ResponseBase<bool>();
            try
            {
                IEnumerable<TEntity> toRemove = await _repository.Read(expression).ConfigureAwait(true);
                response.Data = await _repository.Delete(toRemove).ConfigureAwait(true);
                // Audit
                auditMessage.OldValue = toRemove.GetSerializedEntity();
                auditMessage.LogMessage.FormatMessage(LogType.Information, $"{this.GetType().Namespace}.{this.GetType().Name}", "");
                await _auditManager.SaveAuditAsync(auditMessage);
                return response;
            }
            catch (Exception ex)
            {
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<bool>(true, ex, HttpStatusCode.InternalServerError, _log);
            }

        }

        public async Task<ResponseBase<PagedResult<TEntity>>> Read(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page = 0, int size = 0)
        {
            ResponseBase<PagedResult<TEntity>> response = new ResponseBase<PagedResult<TEntity>>();
            try
            {
                response.Data = await _repository.Read(orderBy, page, size).ConfigureAwait(true);
                return response;
            }
            catch (Exception ex)
            {
                AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<PagedResult<TEntity>>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }

        public async Task<ResponseBase<PagedResult<TEntity>>> Read(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int page = 0, int size = 0)
        {
            ResponseBase<PagedResult<TEntity>> response = new ResponseBase<PagedResult<TEntity>>();
            try
            {
                response.Data = await _repository.Read(expression, include, orderBy, page, size).ConfigureAwait(true);
                return response;
            }
            catch (Exception ex)
            {
                AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<PagedResult<TEntity>>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }

        public async Task<ResponseBase<List<TEntity>>> Read(Expression<Func<TEntity, bool>> expression)
        {

            ResponseBase<List<TEntity>> response = new ResponseBase<List<TEntity>>();
            try
            {
                response.Data = await _repository.Read(expression).ConfigureAwait(true);
                return response;
            }
            catch (Exception ex)
            {
                AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<List<TEntity>>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }


        public async Task<ResponseBase<PagedResult<TEntity>>> ReadWithInclude(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            ResponseBase<PagedResult<TEntity>> response = new ResponseBase<PagedResult<TEntity>>();
            try
            {
                response.Data = await _repository.Read(expression, include, null).ConfigureAwait(true);
                return response;
            }
            catch (Exception ex)
            {
                AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<PagedResult<TEntity>>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }

        public async Task<ResponseBase<List<TEntity>>> ReadAll()
        {
            ResponseBase<List<TEntity>> response = new ResponseBase<List<TEntity>>();
            try
            {
                response.Data = await _repository.ReadAll().ConfigureAwait(true);
                return response;
            }
            catch (Exception ex)
            {
                AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<List<TEntity>>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }

        public async Task<ResponseBase<bool>> Update(TEntity newEntity, TEntity oldEntity)
        {
            AuditMessage auditMessage = new AuditMessage(this.GetType().Name);
            ResponseBase<bool> response = new ResponseBase<bool>();
            try
            {
                response.Data = await _repository.Update(newEntity).ConfigureAwait(true);
                // Audit
                auditMessage.NewValue = newEntity.GetSerializedEntity();
                auditMessage.OldValue = oldEntity?.GetSerializedEntity();
                auditMessage.LogMessage.FormatMessage(LogType.Information, $"{this.GetType().Namespace}.{this.GetType().Name}", "");
                await _auditManager.SaveAuditAsync(auditMessage);
                return response;
            }
            catch (Exception ex)
            {
                auditMessage.LogMessage.FormatMessage(LogType.Error, $"{this.GetType().Namespace}.{this.GetType().Name}", ex.Message);
                await _auditManager.SaveErrorAsync(auditMessage);
                return GenericUtility.ResponseBaseCatch<bool>(true, ex, HttpStatusCode.InternalServerError, _log);
            }
        }
    }
}
