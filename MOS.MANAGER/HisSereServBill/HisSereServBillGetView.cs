using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServBill
{
    partial class HisSereServBillGet : BusinessBase
    {
        internal List<V_HIS_SERE_SERV_BILL> GetView(HisSereServBillViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServBillDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_BILL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSereServBillViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_BILL GetViewById(long id, HisSereServBillViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServBillDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
