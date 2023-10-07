using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestBlood
{
    public partial class HisImpMestBloodDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST_BLOOD> GetView(HisImpMestBloodSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST_BLOOD> result = new List<V_HIS_IMP_MEST_BLOOD>();
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

        public V_HIS_IMP_MEST_BLOOD GetViewById(long id, HisImpMestBloodSO search)
        {
            V_HIS_IMP_MEST_BLOOD result = null;

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
