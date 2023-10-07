using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareSum
{
    public partial class HisCareSumDAO : EntityBase
    {
        private HisCareSumGet GetWorker
        {
            get
            {
                return (HisCareSumGet)Worker.Get<HisCareSumGet>();
            }
        }
        public List<HIS_CARE_SUM> Get(HisCareSumSO search, CommonParam param)
        {
            List<HIS_CARE_SUM> result = new List<HIS_CARE_SUM>();
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

        public HIS_CARE_SUM GetById(long id, HisCareSumSO search)
        {
            HIS_CARE_SUM result = null;
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
