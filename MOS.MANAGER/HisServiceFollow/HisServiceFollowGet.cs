using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceFollow
{
    partial class HisServiceFollowGet : BusinessBase
    {
        internal HisServiceFollowGet()
            : base()
        {

        }

        internal HisServiceFollowGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_FOLLOW> Get(HisServiceFollowFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceFollowDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_FOLLOW GetById(long id)
        {
            try
            {
                return GetById(id, new HisServiceFollowFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_FOLLOW GetById(long id, HisServiceFollowFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServiceFollowDAO.GetById(id, filter.Query());
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
