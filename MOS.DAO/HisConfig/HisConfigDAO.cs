using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisConfig
{
    public partial class HisConfigDAO : EntityBase
    {
        private HisConfigGet GetWorker
        {
            get
            {
                return (HisConfigGet)Worker.Get<HisConfigGet>();
            }
        }
        public List<HIS_CONFIG> Get(HisConfigSO search, CommonParam param)
        {
            List<HIS_CONFIG> result = new List<HIS_CONFIG>();
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

        public HIS_CONFIG GetById(long id, HisConfigSO search)
        {
            HIS_CONFIG result = null;
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
