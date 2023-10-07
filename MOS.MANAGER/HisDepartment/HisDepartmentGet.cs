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
        internal HisDepartmentGet()
            : base()
        {

        }

        internal HisDepartmentGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DEPARTMENT> Get(HisDepartmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisDepartmentFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT GetById(long id, HisDepartmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDepartmentFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPARTMENT GetByCode(string code, HisDepartmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepartmentDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_DEPARTMENT> GetByBranchId(long id)
        {
            try
            {
                HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                filter.BRANCH_ID = id;
                return this.Get(filter);
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
