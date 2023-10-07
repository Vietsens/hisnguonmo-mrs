using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodType
{
    partial class HisBloodTypeGet : BusinessBase
    {
        internal HisBloodTypeGet()
            : base()
        {

        }

        internal HisBloodTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD_TYPE> Get(HisBloodTypeFilterQuery filter)
        {
            try
            {
                List<HIS_BLOOD_TYPE> result= DAOWorker.HisBloodTypeDAO.Get(filter.Query(), param);
                if (IsNotNullOrEmpty(result))
                {
                    result.ForEach(o =>
                    {
                        o.HIS_BLOOD_TYPE1 = null;
                        o.HIS_BLOOD_TYPE2 = null;
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_TYPE GetById(long id, HisBloodTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BLOOD_TYPE> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisBloodTypeFilterQuery filter = new HisBloodTypeFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }

        internal List<HIS_BLOOD_TYPE> GetByParentId(long parentId)
        {
            HisBloodTypeFilterQuery filter = new HisBloodTypeFilterQuery();
            filter.PARENT_ID = parentId;
            return this.Get(filter);
        }
    }
}
