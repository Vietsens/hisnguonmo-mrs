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
        internal List<V_HIS_EXP_MEST_CHMS_2> GetChmsView2(HisExpMestChmsView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_2 GetChmsView2ById(long id)
        {
            try
            {
                return GetChmsView2ById(id, new HisExpMestChmsView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_2 GetChmsView2ById(long id, HisExpMestChmsView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView2ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string code)
        {
            try
            {
                return GetChmsView2ByCode(code, new HisExpMestChmsView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string code, HisExpMestChmsView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView2ByCode(code, filter.Query());
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
