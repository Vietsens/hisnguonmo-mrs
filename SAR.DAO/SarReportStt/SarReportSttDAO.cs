using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportStt
{
    public partial class SarReportSttDAO : EntityBase
    {
        private SarReportSttGet GetWorker
        {
            get
            {
                return (SarReportSttGet)Worker.Get<SarReportSttGet>();
            }
        }

        public List<SAR_REPORT_STT> Get(SarReportSttSO search, CommonParam param)
        {
            List<SAR_REPORT_STT> result = new List<SAR_REPORT_STT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public SAR_REPORT_STT GetById(long id, SarReportSttSO search)
        {
            SAR_REPORT_STT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
