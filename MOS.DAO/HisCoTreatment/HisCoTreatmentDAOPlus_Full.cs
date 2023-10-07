using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCoTreatment
{
    public partial class HisCoTreatmentDAO : EntityBase
    {
        public List<V_HIS_CO_TREATMENT> GetView(HisCoTreatmentSO search, CommonParam param)
        {
            List<V_HIS_CO_TREATMENT> result = new List<V_HIS_CO_TREATMENT>();

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

        public V_HIS_CO_TREATMENT GetViewById(long id, HisCoTreatmentSO search)
        {
            V_HIS_CO_TREATMENT result = null;

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
