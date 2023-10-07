using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodMedi
{
    public partial class HisMestPeriodMediDAO : EntityBase
    {
        private HisMestPeriodMediGet GetWorker
        {
            get
            {
                return (HisMestPeriodMediGet)Worker.Get<HisMestPeriodMediGet>();
            }
        }
        public List<HIS_MEST_PERIOD_MEDI> Get(HisMestPeriodMediSO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_MEDI> result = new List<HIS_MEST_PERIOD_MEDI>();
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

        public HIS_MEST_PERIOD_MEDI GetById(long id, HisMestPeriodMediSO search)
        {
            HIS_MEST_PERIOD_MEDI result = null;
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
