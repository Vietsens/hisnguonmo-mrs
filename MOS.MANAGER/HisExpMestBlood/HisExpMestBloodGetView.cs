using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBlood
{
    partial class HisExpMestBloodGet : BusinessBase
    {
        internal List<V_HIS_EXP_MEST_BLOOD> GetView(HisExpMestBloodViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBloodDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_BLOOD GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestBloodViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_BLOOD GetViewById(long id, HisExpMestBloodViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBloodDAO.GetViewById(id, filter.Query());
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
