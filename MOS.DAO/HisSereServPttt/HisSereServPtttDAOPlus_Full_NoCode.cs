using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServPttt
{
    public partial class HisSereServPtttDAO : EntityBase
    {
        public List<V_HIS_SERE_SERV_PTTT> GetView(HisSereServPtttSO search, CommonParam param)
        {
            List<V_HIS_SERE_SERV_PTTT> result = new List<V_HIS_SERE_SERV_PTTT>();
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

        public V_HIS_SERE_SERV_PTTT GetViewById(long id, HisSereServPtttSO search)
        {
            V_HIS_SERE_SERV_PTTT result = null;

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
