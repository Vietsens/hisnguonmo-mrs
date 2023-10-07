using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBlood
{
    partial class HisBloodGet : BusinessBase
    {
        internal HisBloodGet()
            : base()
        {

        }

        internal HisBloodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD> Get(HisBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BLOOD> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisBloodFilterQuery filter = new HisBloodFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal HIS_BLOOD GetById(long id, HisBloodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BLOOD> GetByBloodTypeId(long id)
        {
            HisBloodFilterQuery filter = new HisBloodFilterQuery();
            filter.BLOOD_TYPE_ID = id;
            return this.Get(filter);
        }
    }
}
