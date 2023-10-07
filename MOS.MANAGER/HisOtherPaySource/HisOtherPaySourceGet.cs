using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOtherPaySource
{
    partial class HisOtherPaySourceGet : BusinessBase
    {
        internal HisOtherPaySourceGet()
            : base()
        {

        }

        internal HisOtherPaySourceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_OTHER_PAY_SOURCE> Get(HisOtherPaySourceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisOtherPaySourceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_OTHER_PAY_SOURCE GetById(long id)
        {
            try
            {
                return GetById(id, new HisOtherPaySourceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_OTHER_PAY_SOURCE GetById(long id, HisOtherPaySourceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisOtherPaySourceDAO.GetById(id, filter.Query());
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
