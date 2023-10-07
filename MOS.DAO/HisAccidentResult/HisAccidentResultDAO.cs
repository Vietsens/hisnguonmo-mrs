using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentResult
{
    public partial class HisAccidentResultDAO : EntityBase
    {
        private HisAccidentResultGet GetWorker
        {
            get
            {
                return (HisAccidentResultGet)Worker.Get<HisAccidentResultGet>();
            }
        }
        public List<HIS_ACCIDENT_RESULT> Get(HisAccidentResultSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_RESULT> result = new List<HIS_ACCIDENT_RESULT>();
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

        public HIS_ACCIDENT_RESULT GetById(long id, HisAccidentResultSO search)
        {
            HIS_ACCIDENT_RESULT result = null;
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
