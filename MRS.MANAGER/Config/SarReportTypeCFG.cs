using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using SAR.EFMODEL.DataModels;
using System.Linq;
using SAR.MANAGER.Core.SarReportType.Get;

namespace MRS.MANAGER.Config
{
    class SarReportTypeCFG
    {
        private static List<SAR_REPORT_TYPE> reportTypeActive;
        public static List<SAR_REPORT_TYPE> REPORT_TYPE_ACTIVE
        {
            get
            {
                if (reportTypeActive == null || reportTypeActive.Count <= 0)
                {
                    reportTypeActive = GetActive();
                }
                return reportTypeActive;
            }
            set
            {
                reportTypeActive = value;
            }
        }

        private static List<SAR_REPORT_TYPE> GetActive()
        {
            List<SAR_REPORT_TYPE> result = null;
            try
            {
                SarReportTypeFilterQuery filter = new SarReportTypeFilterQuery();
                filter.IS_ACTIVE = IMSys.DbConfig.SAR_RS.COMMON.IS_ACTIVE__TRUE;
                result = new SAR.MANAGER.Manager.SarReportTypeManager(new CommonParam()).Get<List<SAR_REPORT_TYPE>>(filter);
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
                reportTypeActive = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
