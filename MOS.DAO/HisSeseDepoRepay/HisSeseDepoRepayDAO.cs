using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSeseDepoRepay
{
    public partial class HisSeseDepoRepayDAO : EntityBase
    {
        private HisSeseDepoRepayGet GetWorker
        {
            get
            {
                return (HisSeseDepoRepayGet)Worker.Get<HisSeseDepoRepayGet>();
            }
        }
        public List<HIS_SESE_DEPO_REPAY> Get(HisSeseDepoRepaySO search, CommonParam param)
        {
            List<HIS_SESE_DEPO_REPAY> result = new List<HIS_SESE_DEPO_REPAY>();
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

        public HIS_SESE_DEPO_REPAY GetById(long id, HisSeseDepoRepaySO search)
        {
            HIS_SESE_DEPO_REPAY result = null;
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
