using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmotionlessResult
{
    public partial class HisEmotionlessResultDAO : EntityBase
    {
        public HIS_EMOTIONLESS_RESULT GetByCode(string code, HisEmotionlessResultSO search)
        {
            HIS_EMOTIONLESS_RESULT result = null;

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

        public Dictionary<string, HIS_EMOTIONLESS_RESULT> GetDicByCode(HisEmotionlessResultSO search, CommonParam param)
        {
            Dictionary<string, HIS_EMOTIONLESS_RESULT> result = new Dictionary<string, HIS_EMOTIONLESS_RESULT>();
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
