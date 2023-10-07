using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodAbo
{
    partial class HisBloodAboGet : BusinessBase
    {
        internal HIS_BLOOD_ABO GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBloodAboFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_ABO GetByCode(string code, HisBloodAboFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodAboDAO.GetByCode(code, filter.Query());
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
