using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestInveUser
{
    partial class HisMestInveUserGet : BusinessBase
    {
        internal HisMestInveUserGet()
            : base()
        {

        }

        internal HisMestInveUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_INVE_USER> Get(HisMestInveUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestInveUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_INVE_USER> GetByMestInventoryId(long mestInventoryId)
        {
            try
            {
                HisMestInveUserFilterQuery filter = new HisMestInveUserFilterQuery();
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_INVE_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestInveUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_INVE_USER GetById(long id, HisMestInveUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestInveUserDAO.GetById(id, filter.Query());
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
