using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSereServDebt
{
    public partial class HisSereServDebtDAO : EntityBase
    {
        public List<V_HIS_SERE_SERV_DEBT> GetView(HisSereServDebtSO search, CommonParam param)
        {
            List<V_HIS_SERE_SERV_DEBT> result = new List<V_HIS_SERE_SERV_DEBT>();
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

        public V_HIS_SERE_SERV_DEBT GetViewById(long id, HisSereServDebtSO search)
        {
            V_HIS_SERE_SERV_DEBT result = null;

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
