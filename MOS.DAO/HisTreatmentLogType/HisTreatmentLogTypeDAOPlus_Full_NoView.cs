using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentLogType
{
    public partial class HisTreatmentLogTypeDAO : EntityBase
    {
        public HIS_TREATMENT_LOG_TYPE GetByCode(string code, HisTreatmentLogTypeSO search)
        {
            HIS_TREATMENT_LOG_TYPE result = null;

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

        public Dictionary<string, HIS_TREATMENT_LOG_TYPE> GetDicByCode(HisTreatmentLogTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT_LOG_TYPE> result = new Dictionary<string, HIS_TREATMENT_LOG_TYPE>();
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
