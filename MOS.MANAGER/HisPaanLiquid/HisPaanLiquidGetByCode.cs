using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPaanLiquid
{
    partial class HisPaanLiquidGet : BusinessBase
    {
        internal HIS_PAAN_LIQUID GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPaanLiquidFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_LIQUID GetByCode(string code, HisPaanLiquidFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanLiquidDAO.GetByCode(code, filter.Query());
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
