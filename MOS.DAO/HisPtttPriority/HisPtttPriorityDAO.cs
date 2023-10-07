using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttPriority
{
    public partial class HisPtttPriorityDAO : EntityBase
    {
        private HisPtttPriorityGet GetWorker
        {
            get
            {
                return (HisPtttPriorityGet)Worker.Get<HisPtttPriorityGet>();
            }
        }
        public List<HIS_PTTT_PRIORITY> Get(HisPtttPrioritySO search, CommonParam param)
        {
            List<HIS_PTTT_PRIORITY> result = new List<HIS_PTTT_PRIORITY>();
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

        public HIS_PTTT_PRIORITY GetById(long id, HisPtttPrioritySO search)
        {
            HIS_PTTT_PRIORITY result = null;
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
