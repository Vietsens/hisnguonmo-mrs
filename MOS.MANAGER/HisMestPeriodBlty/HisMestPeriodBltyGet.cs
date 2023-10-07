using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlty
{
    partial class HisMestPeriodBltyGet : BusinessBase
    {
        internal HisMestPeriodBltyGet()
            : base()
        {

        }

        internal HisMestPeriodBltyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_BLTY> Get(HisMestPeriodBltyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodBltyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_BLTY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodBltyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_BLTY GetById(long id, HisMestPeriodBltyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodBltyDAO.GetById(id, filter.Query());
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
