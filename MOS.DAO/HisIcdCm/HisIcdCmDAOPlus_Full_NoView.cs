using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcdCm
{
    public partial class HisIcdCmDAO : EntityBase
    {
        public HIS_ICD_CM GetByCode(string code, HisIcdCmSO search)
        {
            HIS_ICD_CM result = null;

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

        public Dictionary<string, HIS_ICD_CM> GetDicByCode(HisIcdCmSO search, CommonParam param)
        {
            Dictionary<string, HIS_ICD_CM> result = new Dictionary<string, HIS_ICD_CM>();
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
