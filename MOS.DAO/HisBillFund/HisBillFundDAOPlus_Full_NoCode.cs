using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBillFund
{
    public partial class HisBillFundDAO : EntityBase
    {
        public List<V_HIS_BILL_FUND> GetView(HisBillFundSO search, CommonParam param)
        {
            List<V_HIS_BILL_FUND> result = new List<V_HIS_BILL_FUND>();
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

        public V_HIS_BILL_FUND GetViewById(long id, HisBillFundSO search)
        {
            V_HIS_BILL_FUND result = null;

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
