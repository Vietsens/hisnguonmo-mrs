using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpSource
{
    partial class HisImpSourceGet : BusinessBase
    {
        internal HisImpSourceGet()
            : base()
        {

        }

        internal HisImpSourceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_SOURCE> Get(HisImpSourceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpSourceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_SOURCE GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpSourceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_SOURCE GetById(long id, HisImpSourceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpSourceDAO.GetById(id, filter.Query());
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
