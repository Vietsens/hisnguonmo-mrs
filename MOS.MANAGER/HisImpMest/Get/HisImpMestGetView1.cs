using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMest
{
    partial class HisImpMestGet : GetBase
    {
        internal List<V_HIS_IMP_MEST_1> GetView1(HisImpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisImpMestView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_1 GetView1ById(long id, HisImpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_1 GetView1ByCode(string code)
        {
            try
            {
                return GetView1ByCode(code, new HisImpMestView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_1 GetView1ByCode(string code, HisImpMestView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetView1ByCode(code, filter.Query());
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
