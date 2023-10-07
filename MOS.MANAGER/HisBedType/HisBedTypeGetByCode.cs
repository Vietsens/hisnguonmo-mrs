using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedType
{
    partial class HisBedTypeGet : BusinessBase
    {
        internal HIS_BED_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBedTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BED_TYPE GetByCode(string code, HisBedTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedTypeDAO.GetByCode(code, filter.Query());
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
