using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientTypeAlter
{
    public partial class HisPatientTypeAlterDAO : EntityBase
    {
        public List<V_HIS_PATIENT_TYPE_ALTER> GetView(HisPatientTypeAlterSO search, CommonParam param)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = new List<V_HIS_PATIENT_TYPE_ALTER>();
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

        public V_HIS_PATIENT_TYPE_ALTER GetViewById(long id, HisPatientTypeAlterSO search)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;

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
