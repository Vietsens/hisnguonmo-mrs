using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmotionlessMethod
{
    partial class HisEmotionlessMethodGet : BusinessBase
    {
        internal HisEmotionlessMethodGet()
            : base()
        {

        }

        internal HisEmotionlessMethodGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMOTIONLESS_METHOD> Get(HisEmotionlessMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessMethodDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_METHOD GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmotionlessMethodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_METHOD GetById(long id, HisEmotionlessMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessMethodDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_METHOD GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisEmotionlessMethodFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_METHOD GetByCode(string code, HisEmotionlessMethodFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessMethodDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
