using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodMety
{
    public partial class HisMestPeriodMetyDAO : EntityBase
    {
        private HisMestPeriodMetyGet GetWorker
        {
            get
            {
                return (HisMestPeriodMetyGet)Worker.Get<HisMestPeriodMetyGet>();
            }
        }
        public List<HIS_MEST_PERIOD_METY> Get(HisMestPeriodMetySO search, CommonParam param)
        {
            List<HIS_MEST_PERIOD_METY> result = new List<HIS_MEST_PERIOD_METY>();
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

        public HIS_MEST_PERIOD_METY GetById(long id, HisMestPeriodMetySO search)
        {
            HIS_MEST_PERIOD_METY result = null;
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
