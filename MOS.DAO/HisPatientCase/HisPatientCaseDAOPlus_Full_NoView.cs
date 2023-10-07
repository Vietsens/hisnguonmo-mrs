using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPatientCase
{
    public partial class HisPatientCaseDAO : EntityBase
    {
        public HIS_PATIENT_CASE GetByCode(string code, HisPatientCaseSO search)
        {
            HIS_PATIENT_CASE result = null;

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

        public Dictionary<string, HIS_PATIENT_CASE> GetDicByCode(HisPatientCaseSO search, CommonParam param)
        {
            Dictionary<string, HIS_PATIENT_CASE> result = new Dictionary<string, HIS_PATIENT_CASE>();
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
