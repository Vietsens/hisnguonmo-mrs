using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidType
{
    partial class HisBidTypeGet : BusinessBase
    {
        internal HisBidTypeGet()
            : base()
        {

        }

        internal HisBidTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BID_TYPE> Get(HisBidTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBidTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_TYPE GetById(long id, HisBidTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidTypeDAO.GetById(id, filter.Query());
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
