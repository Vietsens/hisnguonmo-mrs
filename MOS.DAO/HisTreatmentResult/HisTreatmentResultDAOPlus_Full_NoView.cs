using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentResult
{
    public partial class HisTreatmentResultDAO : EntityBase
    {
        public HIS_TREATMENT_RESULT GetByCode(string code, HisTreatmentResultSO search)
        {
            HIS_TREATMENT_RESULT result = null;

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

        public Dictionary<string, HIS_TREATMENT_RESULT> GetDicByCode(HisTreatmentResultSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT_RESULT> result = new Dictionary<string, HIS_TREATMENT_RESULT>();
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
