using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAcinInteractive
{
    public partial class HisAcinInteractiveDAO : EntityBase
    {
        public List<V_HIS_ACIN_INTERACTIVE> GetView(HisAcinInteractiveSO search, CommonParam param)
        {
            List<V_HIS_ACIN_INTERACTIVE> result = new List<V_HIS_ACIN_INTERACTIVE>();
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

        public V_HIS_ACIN_INTERACTIVE GetViewById(long id, HisAcinInteractiveSO search)
        {
            V_HIS_ACIN_INTERACTIVE result = null;

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
