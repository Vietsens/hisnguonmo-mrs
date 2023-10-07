using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAnticipateBlty
{
    public partial class HisAnticipateBltyDAO : EntityBase
    {
        public List<V_HIS_ANTICIPATE_BLTY> GetView(HisAnticipateBltySO search, CommonParam param)
        {
            List<V_HIS_ANTICIPATE_BLTY> result = new List<V_HIS_ANTICIPATE_BLTY>();
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

        public V_HIS_ANTICIPATE_BLTY GetViewById(long id, HisAnticipateBltySO search)
        {
            V_HIS_ANTICIPATE_BLTY result = null;

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
