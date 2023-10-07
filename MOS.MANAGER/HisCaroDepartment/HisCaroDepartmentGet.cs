using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCaroDepartment
{
    partial class HisCaroDepartmentGet : BusinessBase
    {
        internal HisCaroDepartmentGet()
            : base()
        {

        }

        internal HisCaroDepartmentGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARO_DEPARTMENT> Get(HisCaroDepartmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCaroDepartmentDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARO_DEPARTMENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisCaroDepartmentFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARO_DEPARTMENT GetById(long id, HisCaroDepartmentFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCaroDepartmentDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARO_DEPARTMENT> GetByCashierRoomId(long id)
        {
            try
            {
                HisCaroDepartmentFilterQuery filter = new HisCaroDepartmentFilterQuery();
                filter.CASHIER_ROOM_ID = id;
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
