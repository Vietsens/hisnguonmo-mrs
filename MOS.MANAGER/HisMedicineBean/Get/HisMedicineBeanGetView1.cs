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
        internal List<V_HIS_MEDICINE_BEAN_1> GetView1(HisMedicineBeanView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDICINE_BEAN_1> GetView1ByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisMedicineBeanView1FilterQuery filter = new HisMedicineBeanView1FilterQuery();
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
   
        internal V_HIS_MEDICINE_BEAN_1 GetView1ById(long id)
        {
            try
            {
                return GetView1ById(id, new HisMedicineBeanView1FilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDICINE_BEAN_1 GetView1ById(long id, HisMedicineBeanView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineBeanDAO.GetView1ById(id, filter.Query());
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
