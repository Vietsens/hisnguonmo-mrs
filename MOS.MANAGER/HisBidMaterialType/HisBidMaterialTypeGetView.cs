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
        internal List<V_HIS_BID_MATERIAL_TYPE> GetView(HisBidMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMaterialTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BID_MATERIAL_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBidMaterialTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BID_MATERIAL_TYPE GetViewById(long id, HisBidMaterialTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidMaterialTypeDAO.GetViewById(id, filter.Query());
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
