using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTemp
{
    partial class HisSereServTempGet : BusinessBase
    {
        internal HIS_SERE_SERV_TEMP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisSereServTempFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_TEMP GetByCode(string code, HisSereServTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServTempDAO.GetByCode(code, filter.Query());
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
