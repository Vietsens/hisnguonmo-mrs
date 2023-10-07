using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientType
{
    public partial class HisPatientTypeDAO : EntityBase
    {
        public HIS_PATIENT_TYPE GetByCode(string code, HisPatientTypeSO search)
        {
            HIS_PATIENT_TYPE result = null;

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

        public Dictionary<string, HIS_PATIENT_TYPE> GetDicByCode(HisPatientTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT_TYPE> result = new Dictionary<string, HIS_PATIENT_TYPE>();
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
