using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisConfig
{
    partial class HisConfigGet : BusinessBase
    {
        internal HisConfigGet()
            : base()
        {

        }

        internal HisConfigGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CONFIG> Get(HisConfigFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisConfigDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CONFIG GetById(long id)
        {
            try
            {
                return GetById(id, new HisConfigFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CONFIG GetById(long id, HisConfigFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisConfigDAO.GetById(id, filter.Query());
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
