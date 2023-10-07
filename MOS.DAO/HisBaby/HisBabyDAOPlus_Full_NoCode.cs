using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBaby
{
    public partial class HisBabyDAO : EntityBase
    {
        public List<V_HIS_BABY> GetView(HisBabySO search, CommonParam param)
        {
            List<V_HIS_BABY> result = new List<V_HIS_BABY>();
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

        public V_HIS_BABY GetViewById(long id, HisBabySO search)
        {
            V_HIS_BABY result = null;

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
