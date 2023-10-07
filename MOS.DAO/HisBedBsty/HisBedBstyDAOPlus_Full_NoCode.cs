using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedBsty
{
    public partial class HisBedBstyDAO : EntityBase
    {
        public List<V_HIS_BED_BSTY> GetView(HisBedBstySO search, CommonParam param)
        {
            List<V_HIS_BED_BSTY> result = new List<V_HIS_BED_BSTY>();
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

        public V_HIS_BED_BSTY GetViewById(long id, HisBedBstySO search)
        {
            V_HIS_BED_BSTY result = null;

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
