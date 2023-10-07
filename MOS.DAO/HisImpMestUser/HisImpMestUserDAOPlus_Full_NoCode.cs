using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestUser
{
    public partial class HisImpMestUserDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST_USER> GetView(HisImpMestUserSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST_USER> result = new List<V_HIS_IMP_MEST_USER>();
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

        public V_HIS_IMP_MEST_USER GetViewById(long id, HisImpMestUserSO search)
        {
            V_HIS_IMP_MEST_USER result = null;

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
