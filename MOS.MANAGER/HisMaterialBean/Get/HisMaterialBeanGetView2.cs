using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    partial class HisMaterialBeanGet : GetBase
    {
        internal List<V_HIS_MATERIAL_BEAN_2> GetView2(HisMaterialBeanView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_BEAN_2> GetView2ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialBeanView2FilterQuery filter = new HisMaterialBeanView2FilterQuery();
                    filter.IDs = ids;
                    return this.GetView2(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_BEAN_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisMaterialBeanView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_BEAN_2 GetView2ById(long id, HisMaterialBeanView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetView2ById(id, filter.Query());
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
