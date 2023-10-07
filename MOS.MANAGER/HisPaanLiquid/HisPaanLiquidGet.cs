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
        internal HisPaanLiquidGet()
            : base()
        {

        }

        internal HisPaanLiquidGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PAAN_LIQUID> Get(HisPaanLiquidFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanLiquidDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_LIQUID GetById(long id)
        {
            try
            {
                return GetById(id, new HisPaanLiquidFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_LIQUID GetById(long id, HisPaanLiquidFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanLiquidDAO.GetById(id, filter.Query());
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
