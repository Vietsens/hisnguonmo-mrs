using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInfusion
{
    public partial class HisInfusionDAO : EntityBase
    {
        public List<V_HIS_INFUSION> GetView(HisInfusionSO search, CommonParam param)
        {
            List<V_HIS_INFUSION> result = new List<V_HIS_INFUSION>();
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

        public V_HIS_INFUSION GetViewById(long id, HisInfusionSO search)
        {
            V_HIS_INFUSION result = null;

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
