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
        internal List<V_HIS_MATERIAL_1> GetView1(HisMaterialView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisMaterialView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_1 GetView1ById(long id, HisMaterialView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialDAO.GetView1ById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_1> GetView1ByIds(List<long> materialIds)
        {
            if (IsNotNullOrEmpty(materialIds))
            {
                HisMaterialView1FilterQuery filter = new HisMaterialView1FilterQuery();
                filter.IDs = materialIds;
                return this.GetView1(filter);
            }
            return null;
        }
    }
}
