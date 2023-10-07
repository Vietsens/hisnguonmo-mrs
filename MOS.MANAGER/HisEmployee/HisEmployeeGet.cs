using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmployee
{
    partial class HisEmployeeGet : BusinessBase
    {
        internal HisEmployeeGet()
            : base()
        {

        }

        internal HisEmployeeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMPLOYEE> Get(HisEmployeeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmployeeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMPLOYEE GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmployeeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMPLOYEE GetById(long id, HisEmployeeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmployeeDAO.GetById(id, filter.Query());
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
