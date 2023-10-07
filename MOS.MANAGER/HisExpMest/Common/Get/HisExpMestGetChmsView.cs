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
        internal List<V_HIS_EXP_MEST_CHMS> GetChmsView(HisExpMestChmsViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS GetChmsViewById(long id)
        {
            try
            {
                return GetChmsViewById(id, new HisExpMestChmsViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS GetChmsViewById(long id, HisExpMestChmsViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS GetChmsViewByCode(string code)
        {
            try
            {
                return GetChmsViewByCode(code, new HisExpMestChmsViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS GetChmsViewByCode(string code, HisExpMestChmsViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsViewByCode(code, filter.Query());
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
