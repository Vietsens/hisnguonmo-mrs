using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    partial class HisAccidentHurtGet : BusinessBase
    {
        internal HisAccidentHurtGet()
            : base()
        {

        }

        internal HisAccidentHurtGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_HURT> Get(HisAccidentHurtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentHurtFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT GetById(long id, HisAccidentHurtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ACCIDENT_HURT> GetByTreatmentId(long id)
        {
            try
            {
                HisAccidentHurtFilterQuery filter = new HisAccidentHurtFilterQuery();
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

        internal List<HIS_ACCIDENT_HURT> GetByAccidentHurtTypeId(long accidentHurtTypeId)
        {
            try
            {
                HisAccidentHurtFilterQuery filter = new HisAccidentHurtFilterQuery();
                filter.ACCIDENT_HURT_TYPE_ID = accidentHurtTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ACCIDENT_HURT> GetByAccidentCareId(long id)
        {
            HisAccidentHurtFilterQuery filter = new HisAccidentHurtFilterQuery();
            filter.ACCIDENT_CARE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_ACCIDENT_HURT> GetByAccidentHelmetId(long id)
        {
            HisAccidentHurtFilterQuery filter = new HisAccidentHurtFilterQuery();
            filter.ACCIDENT_HELMET_ID = id;
            return this.Get(filter);
        }
    }
}
