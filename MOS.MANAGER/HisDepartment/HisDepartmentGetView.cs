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
        internal List<V_HIS_DEPARTMENT> GetView(HisDepartmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisDepartmentViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT GetViewById(long id, HisDepartmentViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT> GetViewByBranchId(long id)
        {
            try
            {
                HisDepartmentViewFilterQuery filter = new HisDepartmentViewFilterQuery();
                filter.BRANCH_ID = id;
                return this.GetView(filter);
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
