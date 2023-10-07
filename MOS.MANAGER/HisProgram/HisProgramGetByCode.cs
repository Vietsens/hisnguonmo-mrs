using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisProgram
{
    partial class HisProgramGet : BusinessBase
    {
        internal HIS_PROGRAM GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisProgramFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PROGRAM GetByCode(string code, HisProgramFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisProgramDAO.GetByCode(code, filter.Query());
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
