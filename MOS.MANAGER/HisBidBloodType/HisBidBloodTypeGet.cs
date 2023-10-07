using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidBloodType
{
    partial class HisBidBloodTypeGet : BusinessBase
    {
        internal HisBidBloodTypeGet()
            : base()
        {

        }

        internal HisBidBloodTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BID_BLOOD_TYPE> Get(HisBidBloodTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidBloodTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_BLOOD_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBidBloodTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_BLOOD_TYPE GetById(long id, HisBidBloodTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidBloodTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BID_BLOOD_TYPE> GetByBloodTypeId(long id)
        {
            HisBidBloodTypeFilterQuery filter = new HisBidBloodTypeFilterQuery();
            filter.BLOOD_TYPE_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_BID_BLOOD_TYPE> GetByBidId(long bidId)
        {
            HisBidBloodTypeFilterQuery filter = new HisBidBloodTypeFilterQuery();
            filter.BID_ID = bidId;
            return this.Get(filter);
        }
    }
}
