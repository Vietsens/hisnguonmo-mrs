using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRemuneration
{
    public partial class HisRemunerationDAO : EntityBase
    {
        public List<V_HIS_REMUNERATION> GetView(HisRemunerationSO search, CommonParam param)
        {
            List<V_HIS_REMUNERATION> result = new List<V_HIS_REMUNERATION>();
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

        public V_HIS_REMUNERATION GetViewById(long id, HisRemunerationSO search)
        {
            V_HIS_REMUNERATION result = null;

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
