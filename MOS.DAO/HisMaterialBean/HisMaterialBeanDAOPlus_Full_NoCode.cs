using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterialBean
{
    public partial class HisMaterialBeanDAO : EntityBase
    {
        public List<V_HIS_MATERIAL_BEAN> GetView(HisMaterialBeanSO search, CommonParam param)
        {
            List<V_HIS_MATERIAL_BEAN> result = new List<V_HIS_MATERIAL_BEAN>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_MATERIAL_BEAN GetViewById(long id, HisMaterialBeanSO search)
        {
            V_HIS_MATERIAL_BEAN result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
