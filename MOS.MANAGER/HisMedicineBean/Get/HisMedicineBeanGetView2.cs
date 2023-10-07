using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineBean
{
    partial class HisMedicineBeanGet : GetBase
    {
        internal List<V_HIS_MEDICINE_BEAN_2> GetView2(HisMedicineBeanView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetView2(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_BEAN_2> GetView2ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineBeanView2FilterQuery filter = new HisMedicineBeanView2FilterQuery();
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
   
        internal V_HIS_MEDICINE_BEAN_2 GetView2ById(long id)
        {
            try
            {
                return GetView2ById(id, new HisMedicineBeanView2FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_BEAN_2 GetView2ById(long id, HisMedicineBeanView2FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetView2ById(id, filter.Query());
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
