using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBidMaterialType
{
    partial class HisBidMaterialTypeGet : BusinessBase
    {
        internal HisBidMaterialTypeGet()
            : base()
        {

        }

        internal HisBidMaterialTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BID_MATERIAL_TYPE> Get(HisBidMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMaterialTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_MATERIAL_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBidMaterialTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BID_MATERIAL_TYPE GetById(long id, HisBidMaterialTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMaterialTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BID_MATERIAL_TYPE> GetByBidId(long bidId)
        {
            try
            {
                HisBidMaterialTypeFilterQuery filter = new HisBidMaterialTypeFilterQuery();
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
