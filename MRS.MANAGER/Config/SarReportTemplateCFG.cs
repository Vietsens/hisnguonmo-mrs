using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using SAR.EFMODEL.DataModels;
using System.Linq;
using SAR.MANAGER.Core.SarReportTemplate.Get;

namespace MRS.MANAGER.Config
{
    class SarReportTemplateCFG
    {
        private static List<SAR_REPORT_TEMPLATE> reportTemplateActive;
        public static List<SAR_REPORT_TEMPLATE> REPORT_TEMPLATE_ACTIVE
        {
            get
            {
                if (reportTemplateActive == null)
                {
                    reportTemplateActive = GetActive();
                }
                return reportTemplateActive;
            }
            set
            {
                reportTemplateActive = value;
            }
        }

        private static List<SAR_REPORT_TEMPLATE> GetActive()
        {
            List<SAR_REPORT_TEMPLATE> result = null;
            try
            {
                SarReportTemplateFilterQuery filter = new SarReportTemplateFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.SAR_RS.COMMON.IS_ACTIVE__TRUE;
                result = new SAR.MANAGER.Manager.SarReportTemplateManager(new CommonParam()).Get<List<SAR_REPORT_TEMPLATE>>(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                reportTemplateActive = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
