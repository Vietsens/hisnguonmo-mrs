using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmotionlessResult
{
    public partial class HisEmotionlessResultDAO : EntityBase
    {
        private HisEmotionlessResultGet GetWorker
        {
            get
            {
                return (HisEmotionlessResultGet)Worker.Get<HisEmotionlessResultGet>();
            }
        }
        public List<HIS_EMOTIONLESS_RESULT> Get(HisEmotionlessResultSO search, CommonParam param)
        {
            List<HIS_EMOTIONLESS_RESULT> result = new List<HIS_EMOTIONLESS_RESULT>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_EMOTIONLESS_RESULT GetById(long id, HisEmotionlessResultSO search)
        {
            HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
