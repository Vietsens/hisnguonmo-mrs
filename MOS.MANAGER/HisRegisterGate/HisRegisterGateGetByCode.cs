using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterGate
{
    partial class HisRegisterGateGet : BusinessBase
    {
        internal HIS_REGISTER_GATE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisRegisterGateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REGISTER_GATE GetByCode(string code, HisRegisterGateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRegisterGateDAO.GetByCode(code, filter.Query());
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
