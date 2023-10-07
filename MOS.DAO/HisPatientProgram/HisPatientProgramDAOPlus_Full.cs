using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientProgram
{
    public partial class HisPatientProgramDAO : EntityBase
    {
        public List<V_HIS_PATIENT_PROGRAM> GetView(HisPatientProgramSO search, CommonParam param)
        {
            List<V_HIS_PATIENT_PROGRAM> result = new List<V_HIS_PATIENT_PROGRAM>();

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

        public HIS_PATIENT_PROGRAM GetByCode(string code, HisPatientProgramSO search)
        {
            HIS_PATIENT_PROGRAM result = null;

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
        
        public V_HIS_PATIENT_PROGRAM GetViewById(long id, HisPatientProgramSO search)
        {
            V_HIS_PATIENT_PROGRAM result = null;

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

        public V_HIS_PATIENT_PROGRAM GetViewByCode(string code, HisPatientProgramSO search)
        {
            V_HIS_PATIENT_PROGRAM result = null;

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

        public Dictionary<string, HIS_PATIENT_PROGRAM> GetDicByCode(HisPatientProgramSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT_PROGRAM> result = new Dictionary<string, HIS_PATIENT_PROGRAM>();
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
