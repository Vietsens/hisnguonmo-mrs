using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMest.Common.Get
{
    partial class HisExpMestGet : GetBase
    {
        internal List<V_HIS_EXP_MEST_MANU> GetManuView(HisExpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetManuView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MANU GetManuViewById(long id)
        {
            try
            {
                return GetManuViewById(id, new HisExpMestManuViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MANU GetManuViewById(long id, HisExpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetManuViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MANU GetManuViewByCode(string code)
        {
            try
            {
                return GetManuViewByCode(code, new HisExpMestManuViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MANU GetManuViewByCode(string code, HisExpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetManuViewByCode(code, filter.Query());
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
