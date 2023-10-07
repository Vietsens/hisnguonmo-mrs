using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMedicineUseForm
{
    public partial class HisMedicineUseFormDAO : EntityBase
    {
        public HIS_MEDICINE_USE_FORM GetByCode(string code, HisMedicineUseFormSO search)
        {
            HIS_MEDICINE_USE_FORM result = null;

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

        public Dictionary<string, HIS_MEDICINE_USE_FORM> GetDicByCode(HisMedicineUseFormSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDICINE_USE_FORM> result = new Dictionary<string, HIS_MEDICINE_USE_FORM>();
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
