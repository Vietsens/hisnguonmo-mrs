using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartment
{
    partial class HisDepartmentGet : GetBase
    {
        internal V_HIS_DEPARTMENT GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisDepartmentViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT GetViewByCode(string code, HisDepartmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetViewByCode(code, filter.Query());
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
