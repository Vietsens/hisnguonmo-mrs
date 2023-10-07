using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisFund
{
    partial class HisFundGet : BusinessBase
    {
        internal HIS_FUND GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisFundFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FUND GetByCode(string code, HisFundFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFundDAO.GetByCode(code, filter.Query());
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
