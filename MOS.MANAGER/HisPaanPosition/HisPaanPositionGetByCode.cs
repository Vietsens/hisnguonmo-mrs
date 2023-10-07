using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPaanPosition
{
    partial class HisPaanPositionGet : BusinessBase
    {
        internal HIS_PAAN_POSITION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPaanPositionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PAAN_POSITION GetByCode(string code, HisPaanPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPaanPositionDAO.GetByCode(code, filter.Query());
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
