using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodGroup
{
    partial class HisBloodGroupGet : BusinessBase
    {
        internal HIS_BLOOD_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBloodGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_GROUP GetByCode(string code, HisBloodGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodGroupDAO.GetByCode(code, filter.Query());
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
