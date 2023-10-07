using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServSegr
{
    class HisServSegrGet : GetBase
    {
        internal HisServSegrGet()
            : base()
        {

        }

        internal HisServSegrGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERV_SEGR> Get(HisServSegrFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServSegrDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERV_SEGR> GetView(HisServSegrViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServSegrDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERV_SEGR GetById(long id)
        {
            try
            {
                return GetById(id, new HisServSegrFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERV_SEGR GetById(long id, HisServSegrFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServSegrDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERV_SEGR GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServSegrViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERV_SEGR GetViewById(long id, HisServSegrViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServSegrDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERV_SEGR> GetByServiceId(long serviceId)
        {
            try
            {
                HisServSegrFilterQuery filter = new HisServSegrFilterQuery();
                filter.SERVICE_ID = serviceId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERV_SEGR> GetByServiceGroupId(long serviceGroupId)
        {
            try
            {
                HisServSegrFilterQuery filter = new HisServSegrFilterQuery();
                filter.SERVICE_GROUP_ID = serviceGroupId;
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
