using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBlood
{
    partial class HisBloodGet : BusinessBase
    {
        internal V_HIS_BLOOD GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisBloodViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BLOOD GetViewByCode(string code, HisBloodViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodDAO.GetViewByCode(code, filter.Query());
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
