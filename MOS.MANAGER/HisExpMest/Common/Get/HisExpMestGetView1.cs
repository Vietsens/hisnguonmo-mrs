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
        internal List<V_HIS_EXP_MEST_1> GetView1(HisExpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisExpMestView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_1 GetView1ById(long id, HisExpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_1 GetView1ByCode(string code)
        {
            try
            {
                return GetView1ByCode(code, new HisExpMestView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_1 GetView1ByCode(string code, HisExpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestDAO.GetView1ByCode(code, filter.Query());
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
