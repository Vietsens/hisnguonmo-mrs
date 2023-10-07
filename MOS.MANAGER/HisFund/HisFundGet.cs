using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisFund
{
    partial class HisFundGet : BusinessBase
    {
        internal HisFundGet()
            : base()
        {

        }

        internal HisFundGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_FUND> Get(HisFundFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFundDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FUND GetById(long id)
        {
            try
            {
                return GetById(id, new HisFundFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_FUND GetById(long id, HisFundFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisFundDAO.GetById(id, filter.Query());
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
