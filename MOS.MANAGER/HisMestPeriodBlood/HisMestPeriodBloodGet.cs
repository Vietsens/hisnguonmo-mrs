using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlood
{
    partial class HisMestPeriodBloodGet : BusinessBase
    {
        internal HisMestPeriodBloodGet()
            : base()
        {

        }

        internal HisMestPeriodBloodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_BLOOD> Get(HisMestPeriodBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodBloodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_BLOOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodBloodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_BLOOD GetById(long id, HisMestPeriodBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodBloodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_BLOOD> GetByBloodId(long id)
        {
            HisMestPeriodBloodFilterQuery filter = new HisMestPeriodBloodFilterQuery();
            filter.BLOOD_ID = id;
            return this.Get(filter);
        }
    }
}
