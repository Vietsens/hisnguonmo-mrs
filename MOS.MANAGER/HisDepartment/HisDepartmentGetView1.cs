using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepartment
{
    partial class HisDepartmentGet : GetBase
    {
        internal List<V_HIS_DEPARTMENT_1> GetView1(HisDepartmentView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisDepartmentView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_1 GetView1ById(long id, HisDepartmentView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_1 GetView1ByCode(string code)
        {
            try
            {
                return GetView1ByCode(code, new HisDepartmentView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPARTMENT_1 GetView1ByCode(string code, HisDepartmentView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetView1ByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_DEPARTMENT_1> GetView1ByBranchId(long id)
        {
            try
            {
                HisDepartmentView1FilterQuery filter = new HisDepartmentView1FilterQuery();
                filter.BRANCH_ID = id;
                return this.GetView1(filter);
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
