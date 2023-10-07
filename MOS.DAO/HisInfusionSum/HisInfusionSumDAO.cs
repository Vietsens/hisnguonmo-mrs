using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInfusionSum
{
    public partial class HisInfusionSumDAO : EntityBase
    {
        private HisInfusionSumGet GetWorker
        {
            get
            {
                return (HisInfusionSumGet)Worker.Get<HisInfusionSumGet>();
            }
        }
        public List<HIS_INFUSION_SUM> Get(HisInfusionSumSO search, CommonParam param)
        {
            List<HIS_INFUSION_SUM> result = new List<HIS_INFUSION_SUM>();
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

        public HIS_INFUSION_SUM GetById(long id, HisInfusionSumSO search)
        {
            HIS_INFUSION_SUM result = null;
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
