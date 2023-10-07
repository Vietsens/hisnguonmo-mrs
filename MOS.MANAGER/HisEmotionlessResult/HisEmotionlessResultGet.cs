using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmotionlessResult
{
    partial class HisEmotionlessResultGet : BusinessBase
    {
        internal HisEmotionlessResultGet()
            : base()
        {

        }

        internal HisEmotionlessResultGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EMOTIONLESS_RESULT> Get(HisEmotionlessResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessResultDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_RESULT GetById(long id)
        {
            try
            {
                return GetById(id, new HisEmotionlessResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_RESULT GetById(long id, HisEmotionlessResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessResultDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_RESULT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisEmotionlessResultFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EMOTIONLESS_RESULT GetByCode(string code, HisEmotionlessResultFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEmotionlessResultDAO.GetByCode(code, filter.Query());
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
