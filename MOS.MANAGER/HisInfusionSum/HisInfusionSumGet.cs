using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInfusionSum
{
    partial class HisInfusionSumGet : BusinessBase
    {
        internal HisInfusionSumGet()
            : base()
        {

        }

        internal HisInfusionSumGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INFUSION_SUM> Get(HisInfusionSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionSumDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INFUSION_SUM GetById(long id)
        {
            try
            {
                return GetById(id, new HisInfusionSumFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INFUSION_SUM GetById(long id, HisInfusionSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInfusionSumDAO.GetById(id, filter.Query());
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
