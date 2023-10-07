using SDA.DAO.StagingObject;
using SDA.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.DAO.SdaNotify
{
    public partial class SdaNotifyDAO : EntityBase
    {
        private SdaNotifyGet GetWorker
        {
            get
            {
                return (SdaNotifyGet)Worker.Get<SdaNotifyGet>();
            }
        }

        public List<SDA_NOTIFY> Get(SdaNotifySO search, CommonParam param)
        {
            List<SDA_NOTIFY> result = new List<SDA_NOTIFY>();
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

        public SDA_NOTIFY GetById(long id, SdaNotifySO search)
        {
            SDA_NOTIFY result = null;
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
