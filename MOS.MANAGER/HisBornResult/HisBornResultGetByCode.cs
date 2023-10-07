using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornResult
{
    partial class HisBornResultGet : BusinessBase
    {
        internal HIS_BORN_RESULT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBornResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_RESULT GetByCode(string code, HisBornResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornResultDAO.GetByCode(code, filter.Query());
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
