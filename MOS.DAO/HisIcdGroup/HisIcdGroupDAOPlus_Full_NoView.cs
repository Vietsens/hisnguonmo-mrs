using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcdGroup
{
    public partial class HisIcdGroupDAO : EntityBase
    {
        public HIS_ICD_GROUP GetByCode(string code, HisIcdGroupSO search)
        {
            HIS_ICD_GROUP result = null;

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

        public Dictionary<string, HIS_ICD_GROUP> GetDicByCode(HisIcdGroupSO search, CommonParam param)
        {
            Dictionary<string, HIS_ICD_GROUP> result = new Dictionary<string, HIS_ICD_GROUP>();
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
