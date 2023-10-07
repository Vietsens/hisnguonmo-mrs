using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodMate
{
    public partial class HisMestPeriodMateDAO : EntityBase
    {
        private HisMestPeriodMateGet GetWorker
        {
            get
            {
                return (HisMestPeriodMateGet)Worker.Get<HisMestPeriodMateGet>();
            }
        }
        public List<HIS_MEST_PERIOD_MATE> Get(HisMestPeriodMateSO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_MATE> result = new List<HIS_MEST_PERIOD_MATE>();
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

        public HIS_MEST_PERIOD_MATE GetById(long id, HisMestPeriodMateSO search)
        {
            HIS_MEST_PERIOD_MATE result = null;
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
