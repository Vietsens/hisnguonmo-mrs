using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornPosition
{
    partial class HisBornPositionGet : BusinessBase
    {
        internal HIS_BORN_POSITION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBornPositionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_POSITION GetByCode(string code, HisBornPositionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornPositionDAO.GetByCode(code, filter.Query());
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
