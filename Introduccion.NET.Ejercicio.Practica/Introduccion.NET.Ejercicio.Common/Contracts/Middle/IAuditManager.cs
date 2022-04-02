using Introduccion.NET.Ejercicio.Common.Entities;

namespace Introduccion.NET.Ejercicio.Common.Contracts.Middle
{
    public interface IAuditManager
    {
        Task SaveErrorAsync(AuditMessage model);
        Task SaveAuditAsync(AuditMessage model);
        Task SaveTrackingAsync(AuditMessage model);

    }
}
