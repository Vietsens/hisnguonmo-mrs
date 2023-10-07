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
        internal List<V_HIS_IMP_MEST_MANU> GetManuView(HisImpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetManuView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MANU GetManuViewById(long id)
        {
            try
            {
                return GetManuViewById(id, new HisImpMestManuViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MANU GetManuViewById(long id, HisImpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetManuViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MANU GetManuViewByCode(string code)
        {
            try
            {
                return GetManuViewByCode(code, new HisImpMestManuViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_IMP_MEST_MANU GetManuViewByCode(string code, HisImpMestManuViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestDAO.GetManuViewByCode(code, filter.Query());
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
