using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPeriodMety
{
    public partial class HisMestPeriodMetyDAO : EntityBase
    {
        public List<V_HIS_MEST_PERIOD_METY> GetView(HisMestPeriodMetySO search, CommonParam param)
        {
            List<V_HIS_MEST_PERIOD_METY> result = new List<V_HIS_MEST_PERIOD_METY>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_MEST_PERIOD_METY GetViewById(long id, HisMestPeriodMetySO search)
        {
            V_HIS_MEST_PERIOD_METY result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
