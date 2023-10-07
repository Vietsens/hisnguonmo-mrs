using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentType
{
    public partial class HisTreatmentTypeDAO : EntityBase
    {
        public HIS_TREATMENT_TYPE GetByCode(string code, HisTreatmentTypeSO search)
        {
            HIS_TREATMENT_TYPE result = null;

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

        public Dictionary<string, HIS_TREATMENT_TYPE> GetDicByCode(HisTreatmentTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT_TYPE> result = new Dictionary<string, HIS_TREATMENT_TYPE>();
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
