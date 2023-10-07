using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestReason
{
    public partial class HisExpMestReasonDAO : EntityBase
    {
        public HIS_EXP_MEST_REASON GetByCode(string code, HisExpMestReasonSO search)
        {
            HIS_EXP_MEST_REASON result = null;

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

        public Dictionary<string, HIS_EXP_MEST_REASON> GetDicByCode(HisExpMestReasonSO search, CommonParam param)
        {
            Dictionary<string, HIS_EXP_MEST_REASON> result = new Dictionary<string, HIS_EXP_MEST_REASON>();
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
