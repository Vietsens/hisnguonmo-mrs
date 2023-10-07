using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestMaterial
{
    public partial class HisImpMestMaterialDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST_MATERIAL_3> GetView3(HisImpMestMaterialSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST_MATERIAL_3> result = new List<V_HIS_IMP_MEST_MATERIAL_3>();
            try
            {
                result = GetWorker.GetView3(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_IMP_MEST_MATERIAL_3 GetView3ById(long id, HisImpMestMaterialSO search)
        {
            V_HIS_IMP_MEST_MATERIAL_3 result = null;

            try
            {
                result = GetWorker.GetView3ById(id, search);
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
