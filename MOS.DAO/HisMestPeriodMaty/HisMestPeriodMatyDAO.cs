using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodMaty
{
    public partial class HisMestPeriodMatyDAO : EntityBase
    {
        private HisMestPeriodMatyGet GetWorker
        {
            get
            {
                return (HisMestPeriodMatyGet)Worker.Get<HisMestPeriodMatyGet>();
            }
        }
        public List<HIS_MEST_PERIOD_MATY> Get(HisMestPeriodMatySO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_MATY> result = new List<HIS_MEST_PERIOD_MATY>();
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

        public HIS_MEST_PERIOD_MATY GetById(long id, HisMestPeriodMatySO search)
        {
            HIS_MEST_PERIOD_MATY result = null;
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
