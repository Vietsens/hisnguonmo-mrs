using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestUser
{
    partial class HisExpMestUserGet : BusinessBase
    {
        internal List<V_HIS_EXP_MEST_USER> GetView(HisExpMestUserViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestUserDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_USER GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestUserViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_USER GetViewById(long id, HisExpMestUserViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestUserDAO.GetViewById(id, filter.Query());
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
