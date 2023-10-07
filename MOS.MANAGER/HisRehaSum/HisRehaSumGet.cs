using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRehaSum
{
    class HisRehaSumGet : BusinessBase
    {
        internal HisRehaSumGet()
            : base()
        {

        }

        internal HisRehaSumGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_REHA_SUM> Get(HisRehaSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaSumDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_SUM GetById(long id)
        {
            try
            {
                return GetById(id, new HisRehaSumFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_REHA_SUM GetById(long id, HisRehaSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRehaSumDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal List<HIS_REHA_SUM> GetByTreatmentId(long id)
        {
            try
            {
                HisRehaSumFilterQuery filter = new HisRehaSumFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
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
