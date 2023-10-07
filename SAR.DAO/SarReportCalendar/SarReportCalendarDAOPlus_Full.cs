using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportCalendar
{
    public partial class SarReportCalendarDAO : EntityBase
    {
        public List<V_SAR_REPORT_CALENDAR> GetView(SarReportCalendarSO search, CommonParam param)
        {
            List<V_SAR_REPORT_CALENDAR> result = new List<V_SAR_REPORT_CALENDAR>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public SAR_REPORT_CALENDAR GetByCode(string code, SarReportCalendarSO search)
        {
            SAR_REPORT_CALENDAR result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_SAR_REPORT_CALENDAR GetViewById(long id, SarReportCalendarSO search)
        {
            V_SAR_REPORT_CALENDAR result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_SAR_REPORT_CALENDAR GetViewByCode(string code, SarReportCalendarSO search)
        {
            V_SAR_REPORT_CALENDAR result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, SAR_REPORT_CALENDAR> GetDicByCode(SarReportCalendarSO search, CommonParam param)
        {
            Dictionary<string, SAR_REPORT_CALENDAR> result = new Dictionary<string, SAR_REPORT_CALENDAR>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public bool ExistsCode(string code, long? id)
        {
            try
            {
                return CheckWorker.ExistsCode(code, id);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                throw;
            }
        }
    }
}
