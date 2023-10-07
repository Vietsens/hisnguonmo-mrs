using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareDetail
{
    partial class HisCareDetailGet : BusinessBase
    {
        internal List<V_HIS_CARE_DETAIL> GetView(HisCareDetailViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDetailDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARE_DETAIL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisCareDetailViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARE_DETAIL GetViewById(long id, HisCareDetailViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareDetailDAO.GetViewById(id, filter.Query());
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
