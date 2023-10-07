using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttCondition
{
    public partial class HisPtttConditionDAO : EntityBase
    {
        private HisPtttConditionGet GetWorker
        {
            get
            {
                return (HisPtttConditionGet)Worker.Get<HisPtttConditionGet>();
            }
        }
        public List<HIS_PTTT_CONDITION> Get(HisPtttConditionSO search, CommonParam param)
        {
            List<HIS_PTTT_CONDITION> result = new List<HIS_PTTT_CONDITION>();
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

        public HIS_PTTT_CONDITION GetById(long id, HisPtttConditionSO search)
        {
            HIS_PTTT_CONDITION result = null;
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
