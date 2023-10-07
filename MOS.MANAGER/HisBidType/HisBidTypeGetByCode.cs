using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidType
{
    partial class HisBidTypeGet : BusinessBase
    {
        internal HIS_BID_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBidTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_TYPE GetByCode(string code, HisBidTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidTypeDAO.GetByCode(code, filter.Query());
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
