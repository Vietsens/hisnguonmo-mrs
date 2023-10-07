using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaLanguage
{
    public partial class SdaLanguageDAO : EntityBase
    {
        private SdaLanguageGet GetWorker
        {
            get
            {
                return (SdaLanguageGet)Worker.Get<SdaLanguageGet>();
            }
        }

        public List<SDA_LANGUAGE> Get(SdaLanguageSO search, CommonParam param)
        {
            List<SDA_LANGUAGE> result = new List<SDA_LANGUAGE>();
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

        public SDA_LANGUAGE GetById(long id, SdaLanguageSO search)
        {
            SDA_LANGUAGE result = null;
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
