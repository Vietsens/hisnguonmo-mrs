using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaConfigApp
{
    public partial class SdaConfigAppDAO : EntityBase
    {
        public SDA_CONFIG_APP GetByCode(string code, SdaConfigAppSO search)
        {
            SDA_CONFIG_APP result = null;

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

        public Dictionary<string, SDA_CONFIG_APP> GetDicByCode(SdaConfigAppSO search, CommonParam param)
        {
            Dictionary<string, SDA_CONFIG_APP> result = new Dictionary<string, SDA_CONFIG_APP>();
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
