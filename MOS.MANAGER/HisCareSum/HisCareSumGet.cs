using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareSum
{
    class HisCareSumGet : BusinessBase
    {
        internal HisCareSumGet()
            : base()
        {

        }

        internal HisCareSumGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARE_SUM> Get(HisCareSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareSumDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_SUM GetById(long id)
        {
            try
            {
                return GetById(id, new HisCareSumFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_SUM GetById(long id, HisCareSumFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareSumDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal List<HIS_CARE_SUM> GetByTreatmentId(long id)
        {
            try
            {
                HisCareSumFilterQuery filter = new HisCareSumFilterQuery();
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
