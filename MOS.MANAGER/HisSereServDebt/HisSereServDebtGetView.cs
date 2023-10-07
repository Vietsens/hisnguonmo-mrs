using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDebt
{
    partial class HisSereServDebtGet : BusinessBase
    {
        internal List<V_HIS_SERE_SERV_DEBT> GetView(HisSereServDebtViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDebtDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_DEBT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSereServDebtViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_DEBT GetViewById(long id, HisSereServDebtViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDebtDAO.GetViewById(id, filter.Query());
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
