using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaTranslate
{
    public partial class SdaTranslateDAO : EntityBase
    {
        private SdaTranslateGet GetWorker
        {
            get
            {
                return (SdaTranslateGet)Worker.Get<SdaTranslateGet>();
            }
        }

        public List<SDA_TRANSLATE> Get(SdaTranslateSO search, CommonParam param)
        {
            List<SDA_TRANSLATE> result = new List<SDA_TRANSLATE>();
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

        public SDA_TRANSLATE GetById(long id, SdaTranslateSO search)
        {
            SDA_TRANSLATE result = null;
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
