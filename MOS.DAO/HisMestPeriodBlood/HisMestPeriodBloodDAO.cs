using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodBlood
{
    public partial class HisMestPeriodBloodDAO : EntityBase
    {
        private HisMestPeriodBloodGet GetWorker
        {
            get
            {
                return (HisMestPeriodBloodGet)Worker.Get<HisMestPeriodBloodGet>();
            }
        }
        public List<HIS_MEST_PERIOD_BLOOD> Get(HisMestPeriodBloodSO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_BLOOD> result = new List<HIS_MEST_PERIOD_BLOOD>();
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

        public HIS_MEST_PERIOD_BLOOD GetById(long id, HisMestPeriodBloodSO search)
        {
            HIS_MEST_PERIOD_BLOOD result = null;
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
