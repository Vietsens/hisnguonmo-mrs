using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportStt
{
    public partial class SarReportSttDAO : EntityBase
    {
        public SAR_REPORT_STT GetByCode(string code, SarReportSttSO search)
        {
            SAR_REPORT_STT result = null;

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

        public Dictionary<string, SAR_REPORT_STT> GetDicByCode(SarReportSttSO search, CommonParam param)
        {
            Dictionary<string, SAR_REPORT_STT> result = new Dictionary<string, SAR_REPORT_STT>();
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
    }
}
