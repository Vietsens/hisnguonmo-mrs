using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestInveUser
{
    public partial class HisMestInveUserDAO : EntityBase
    {
        public List<V_HIS_MEST_INVE_USER> GetView(HisMestInveUserSO search, CommonParam param)
        {
            List<V_HIS_MEST_INVE_USER> result = new List<V_HIS_MEST_INVE_USER>();
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

        public V_HIS_MEST_INVE_USER GetViewById(long id, HisMestInveUserSO search)
        {
            V_HIS_MEST_INVE_USER result = null;

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
