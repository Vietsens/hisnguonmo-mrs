using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmotionlessMethod
{
    public partial class HisEmotionlessMethodDAO : EntityBase
    {
        private HisEmotionlessMethodGet GetWorker
        {
            get
            {
                return (HisEmotionlessMethodGet)Worker.Get<HisEmotionlessMethodGet>();
            }
        }
        public List<HIS_EMOTIONLESS_METHOD> Get(HisEmotionlessMethodSO search, CommonParam param)
        {
            List<HIS_EMOTIONLESS_METHOD> result = new List<HIS_EMOTIONLESS_METHOD>();
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

        public HIS_EMOTIONLESS_METHOD GetById(long id, HisEmotionlessMethodSO search)
        {
            HIS_EMOTIONLESS_METHOD result = null;
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
