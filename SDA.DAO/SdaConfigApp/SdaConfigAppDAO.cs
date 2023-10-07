using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaConfigApp
{
    public partial class SdaConfigAppDAO : EntityBase
    {
        private SdaConfigAppGet GetWorker
        {
            get
            {
                return (SdaConfigAppGet)Worker.Get<SdaConfigAppGet>();
            }
        }

        public List<SDA_CONFIG_APP> Get(SdaConfigAppSO search, CommonParam param)
        {
            List<SDA_CONFIG_APP> result = new List<SDA_CONFIG_APP>();
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

        public SDA_CONFIG_APP GetById(long id, SdaConfigAppSO search)
        {
            SDA_CONFIG_APP result = null;
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
