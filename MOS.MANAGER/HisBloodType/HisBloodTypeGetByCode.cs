using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodType
{
    partial class HisBloodTypeGet : BusinessBase
    {
        internal HIS_BLOOD_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBloodTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_TYPE GetByCode(string code, HisBloodTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodTypeDAO.GetByCode(code, filter.Query());
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
