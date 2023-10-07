using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAwareness
{
    partial class HisAwarenessGet : BusinessBase
    {
        internal HisAwarenessGet()
            : base()
        {

        }

        internal HisAwarenessGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_AWARENESS> Get(HisAwarenessFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAwarenessDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_AWARENESS GetById(long id)
        {
            try
            {
                return GetById(id, new HisAwarenessFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_AWARENESS GetById(long id, HisAwarenessFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAwarenessDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_AWARENESS GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAwarenessFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_AWARENESS GetByCode(string code, HisAwarenessFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAwarenessDAO.GetByCode(code, filter.Query());
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
