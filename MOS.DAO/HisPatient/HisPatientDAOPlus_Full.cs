using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatient
{
    public partial class HisPatientDAO : EntityBase
    {
        public List<V_HIS_PATIENT> GetView(HisPatientSO search, CommonParam param)
        {
            List<V_HIS_PATIENT> result = new List<V_HIS_PATIENT>();

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

        public HIS_PATIENT GetByCode(string code, HisPatientSO search)
        {
            HIS_PATIENT result = null;

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
        
        public V_HIS_PATIENT GetViewById(long id, HisPatientSO search)
        {
            V_HIS_PATIENT result = null;

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

        public V_HIS_PATIENT GetViewByCode(string code, HisPatientSO search)
        {
            V_HIS_PATIENT result = null;

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

        public Dictionary<string, HIS_PATIENT> GetDicByCode(HisPatientSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT> result = new Dictionary<string, HIS_PATIENT>();
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
