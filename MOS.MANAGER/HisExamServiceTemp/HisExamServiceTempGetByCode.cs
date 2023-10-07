using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamServiceTemp
{
    partial class HisExamServiceTempGet : BusinessBase
    {
        internal HIS_EXAM_SERVICE_TEMP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExamServiceTempFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXAM_SERVICE_TEMP GetByCode(string code, HisExamServiceTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamServiceTempDAO.GetByCode(code, filter.Query());
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
