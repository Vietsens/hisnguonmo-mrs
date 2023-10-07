using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBid
{
    partial class HisBidGet : BusinessBase
    {
        internal HisBidGet()
            : base()
        {

        }

        internal HisBidGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BID> Get(HisBidFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID GetById(long id)
        {
            try
            {
                return GetById(id, new HisBidFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID GetById(long id, HisBidFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidDAO.GetById(id, filter.Query());
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
