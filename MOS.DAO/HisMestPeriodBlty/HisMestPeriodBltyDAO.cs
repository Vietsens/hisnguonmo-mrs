using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodBlty
{
    public partial class HisMestPeriodBltyDAO : EntityBase
    {
        private HisMestPeriodBltyGet GetWorker
        {
            get
            {
                return (HisMestPeriodBltyGet)Worker.Get<HisMestPeriodBltyGet>();
            }
        }
        public List<HIS_MEST_PERIOD_BLTY> Get(HisMestPeriodBltySO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_BLTY> result = new List<HIS_MEST_PERIOD_BLTY>();
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

        public HIS_MEST_PERIOD_BLTY GetById(long id, HisMestPeriodBltySO search)
        {
            HIS_MEST_PERIOD_BLTY result = null;
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
