using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class AuditManager : IAuditManager
    {
        enum AuditType
        {
            [Description("PROCESO")]
            Audit = 0,
            [Description("LOG")]
            Tracking = 1,
            [Description("ERROR")]
            Error = 2,

        }

        #region Attributes

        //const string _auditTableName = "sdmaudit"; // Carlos
        //private readonly IConfiguration _configuration; // Carlos
        private readonly bool _auditEnableSaveTracking;
        private readonly bool _auditEnableSaveAudit;
        private readonly string _serviceName;

        #endregion

        #region Constructor

        public AuditManager(IConfiguration configuration)
        {
            IConfiguration _configuration = configuration;
        }

        #endregion

        #region Private Methods


        #endregion

        #region Public Methods

        public async Task SaveAuditAsync(AuditMessage model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SaveErrorAsync(AuditMessage model)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SaveTrackingAsync(AuditMessage model)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        
        #endregion
    }
}