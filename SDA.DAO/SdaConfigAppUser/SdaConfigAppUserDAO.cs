using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaConfigAppUser
{
    public partial class SdaConfigAppUserDAO : EntityBase
    {
        private SdaConfigAppUserGet GetWorker
        {
            get
            {
                return (SdaConfigAppUserGet)Worker.Get<SdaConfigAppUserGet>();
            }
        }

        public List<SDA_CONFIG_APP_USER> Get(SdaConfigAppUserSO search, CommonParam param)
        {
            List<SDA_CONFIG_APP_USER> result = new List<SDA_CONFIG_APP_USER>();
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

        public SDA_CONFIG_APP_USER GetById(long id, SdaConfigAppUserSO search)
        {
            SDA_CONFIG_APP_USER result = null;
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
