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
        internal List<V_HIS_EXP_MEST_CHMS_1> GetChmsView1(HisExpMestChmsView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_1 GetChmsView1ById(long id)
        {
            try
            {
                return GetChmsView1ById(id, new HisExpMestChmsView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_1 GetChmsView1ById(long id, HisExpMestChmsView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_1 GetChmsView1ByCode(string code)
        {
            try
            {
                return GetChmsView1ByCode(code, new HisExpMestChmsView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_CHMS_1 GetChmsView1ByCode(string code, HisExpMestChmsView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetChmsView1ByCode(code, filter.Query());
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
