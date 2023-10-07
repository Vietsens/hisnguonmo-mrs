using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientTypeSub
{
    public partial class HisPatientTypeSubDAO : EntityBase
    {
        public List<V_HIS_PATIENT_TYPE_SUB> GetView(HisPatientTypeSubSO search, CommonParam param)
        {
            List<V_HIS_PATIENT_TYPE_SUB> result = new List<V_HIS_PATIENT_TYPE_SUB>();

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

        public HIS_PATIENT_TYPE_SUB GetByCode(string code, HisPatientTypeSubSO search)
        {
            HIS_PATIENT_TYPE_SUB result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_HIS_PATIENT_TYPE_SUB GetViewById(long id, HisPatientTypeSubSO search)
        {
            V_HIS_PATIENT_TYPE_SUB result = null;

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

        public V_HIS_PATIENT_TYPE_SUB GetViewByCode(string code, HisPatientTypeSubSO search)
        {
            V_HIS_PATIENT_TYPE_SUB result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_PATIENT_TYPE_SUB> GetDicByCode(HisPatientTypeSubSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT_TYPE_SUB> result = new Dictionary<string, HIS_PATIENT_TYPE_SUB>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
