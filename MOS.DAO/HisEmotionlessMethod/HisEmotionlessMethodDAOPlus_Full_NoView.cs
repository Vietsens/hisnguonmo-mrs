using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmotionlessMethod
{
    public partial class HisEmotionlessMethodDAO : EntityBase
    {
        public HIS_EMOTIONLESS_METHOD GetByCode(string code, HisEmotionlessMethodSO search)
        {
            HIS_EMOTIONLESS_METHOD result = null;

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

        public Dictionary<string, HIS_EMOTIONLESS_METHOD> GetDicByCode(HisEmotionlessMethodSO search, CommonParam param)
        {
            Dictionary<string, HIS_EMOTIONLESS_METHOD> result = new Dictionary<string, HIS_EMOTIONLESS_METHOD>();
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
