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
        internal List<V_HIS_MATERIAL_BEAN_1> GetView1(HisMaterialBeanView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MATERIAL_BEAN_1> GetView1ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMaterialBeanView1FilterQuery filter = new HisMaterialBeanView1FilterQuery();
                    filter.IDs = ids;
                    return this.GetView1(filter);
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

        internal V_HIS_MATERIAL_BEAN_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisMaterialBeanView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MATERIAL_BEAN_1 GetView1ById(long id, HisMaterialBeanView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMaterialBeanDAO.GetView1ById(id, filter.Query());
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
