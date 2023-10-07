using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisIcd
{
    public partial class HisIcdDAO : EntityBase
    {
        public HIS_ICD GetByCode(string code, HisIcdSO search)
        {
            HIS_ICD result = null;

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

        public Dictionary<string, HIS_ICD> GetDicByCode(HisIcdSO search, CommonParam param)
        {
            Dictionary<string, HIS_ICD> result = new Dictionary<string, HIS_ICD>();
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
