using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientTypeAllow
{
    public partial class HisPatientTypeAllowDAO : EntityBase
    {
        public List<V_HIS_PATIENT_TYPE_ALLOW> GetView(HisPatientTypeAllowSO search, CommonParam param)
        {
            List<V_HIS_PATIENT_TYPE_ALLOW> result = new List<V_HIS_PATIENT_TYPE_ALLOW>();
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

        public V_HIS_PATIENT_TYPE_ALLOW GetViewById(long id, HisPatientTypeAllowSO search)
        {
            V_HIS_PATIENT_TYPE_ALLOW result = null;

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
