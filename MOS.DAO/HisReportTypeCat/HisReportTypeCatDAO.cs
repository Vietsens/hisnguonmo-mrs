using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisReportTypeCat
{
    public partial class HisReportTypeCatDAO : EntityBase
    {
        private HisReportTypeCatGet GetWorker
        {
            get
            {
                return (HisReportTypeCatGet)Worker.Get<HisReportTypeCatGet>();
            }
        }
        public List<HIS_REPORT_TYPE_CAT> Get(HisReportTypeCatSO search, CommonParam param)
        {
            List<HIS_REPORT_TYPE_CAT> result = new List<HIS_REPORT_TYPE_CAT>();
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

        public HIS_REPORT_TYPE_CAT GetById(long id, HisReportTypeCatSO search)
        {
            HIS_REPORT_TYPE_CAT result = null;
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
