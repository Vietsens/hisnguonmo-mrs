using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatmentEndType
{
    public partial class HisTreatmentEndTypeDAO : EntityBase
    {
        public HIS_TREATMENT_END_TYPE GetByCode(string code, HisTreatmentEndTypeSO search)
        {
            HIS_TREATMENT_END_TYPE result = null;

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

        public Dictionary<string, HIS_TREATMENT_END_TYPE> GetDicByCode(HisTreatmentEndTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT_END_TYPE> result = new Dictionary<string, HIS_TREATMENT_END_TYPE>();
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
