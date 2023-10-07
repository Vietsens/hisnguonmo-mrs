using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMedicineType
{
    partial class HisBidMedicineTypeGet : BusinessBase
    {
        internal HisBidMedicineTypeGet()
            : base()
        {

        }

        internal HisBidMedicineTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BID_MEDICINE_TYPE> Get(HisBidMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMedicineTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_MEDICINE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBidMedicineTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_MEDICINE_TYPE GetById(long id, HisBidMedicineTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMedicineTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BID_MEDICINE_TYPE> GetByBidId(long bidId)
        {
            try
            {
                HisBidMedicineTypeFilterQuery filter = new HisBidMedicineTypeFilterQuery();
                filter.BID_ID = bidId;
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
