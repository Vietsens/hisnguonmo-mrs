using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SAR.DAO.SarReportType
{
    public partial class SarReportTypeDAO : EntityBase
    {
        private SarReportTypeGet GetWorker
        {
            get
            {
                return (SarReportTypeGet)Worker.Get<SarReportTypeGet>();
            }
        }

        public List<SAR_REPORT_TYPE> Get(SarReportTypeSO search, CommonParam param)
        {
            List<SAR_REPORT_TYPE> result = new List<SAR_REPORT_TYPE>();
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

        public SAR_REPORT_TYPE GetById(long id, SarReportTypeSO search)
        {
            SAR_REPORT_TYPE result = null;
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
