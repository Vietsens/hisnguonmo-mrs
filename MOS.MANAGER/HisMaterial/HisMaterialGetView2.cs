using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisMaterialType;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MOS.MANAGER.HisMaterial
{
    partial class HisMaterialGet : GetBase
    {
        internal List<V_HIS_MATERIAL_2> GetView2(HisMaterialView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisMaterialView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_2 GetView2ById(long id, HisMaterialView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetView2ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_2> GetView2ByIds(List<long> materialIds)
        {
            if (IsNotNullOrEmpty(materialIds))
            {
                HisMaterialView2FilterQuery filter = new HisMaterialView2FilterQuery();
                filter.IDs = materialIds;
                return this.GetView2(filter);
            }
            return null;
        }
    }
}
