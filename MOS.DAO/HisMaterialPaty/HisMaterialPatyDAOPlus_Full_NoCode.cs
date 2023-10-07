using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterialPaty
{
    public partial class HisMaterialPatyDAO : EntityBase
    {
        public List<V_HIS_MATERIAL_PATY> GetView(HisMaterialPatySO search, CommonParam param)
        {
            List<V_HIS_MATERIAL_PATY> result = new List<V_HIS_MATERIAL_PATY>();
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

        public V_HIS_MATERIAL_PATY GetViewById(long id, HisMaterialPatySO search)
        {
            V_HIS_MATERIAL_PATY result = null;

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
