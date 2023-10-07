using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentBodyPart
{
    partial class HisAccidentBodyPartGet : BusinessBase
    {
        internal HIS_ACCIDENT_BODY_PART GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentBodyPartFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_BODY_PART GetByCode(string code, HisAccidentBodyPartFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentBodyPartDAO.GetByCode(code, filter.Query());
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
