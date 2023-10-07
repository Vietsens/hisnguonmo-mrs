using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpSource
{
    public partial class HisImpSourceDAO : EntityBase
    {
        public HIS_IMP_SOURCE GetByCode(string code, HisImpSourceSO search)
        {
            HIS_IMP_SOURCE result = null;

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

        public Dictionary<string, HIS_IMP_SOURCE> GetDicByCode(HisImpSourceSO search, CommonParam param)
        {
            Dictionary<string, HIS_IMP_SOURCE> result = new Dictionary<string, HIS_IMP_SOURCE>();
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
