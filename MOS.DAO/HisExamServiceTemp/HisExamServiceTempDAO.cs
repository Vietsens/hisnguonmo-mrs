using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExamServiceTemp
{
    public partial class HisExamServiceTempDAO : EntityBase
    {
        private HisExamServiceTempGet GetWorker
        {
            get
            {
                return (HisExamServiceTempGet)Worker.Get<HisExamServiceTempGet>();
            }
        }
        public List<HIS_EXAM_SERVICE_TEMP> Get(HisExamServiceTempSO search, CommonParam param)
        {
            List<HIS_EXAM_SERVICE_TEMP> result = new List<HIS_EXAM_SERVICE_TEMP>();
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

        public HIS_EXAM_SERVICE_TEMP GetById(long id, HisExamServiceTempSO search)
        {
            HIS_EXAM_SERVICE_TEMP result = null;
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
