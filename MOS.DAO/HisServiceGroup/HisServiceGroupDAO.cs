using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceGroup
{
    public partial class HisServiceGroupDAO : EntityBase
    {
        private HisServiceGroupGet GetWorker
        {
            get
            {
                return (HisServiceGroupGet)Worker.Get<HisServiceGroupGet>();
            }
        }
        public List<HIS_SERVICE_GROUP> Get(HisServiceGroupSO search, CommonParam param)
        {
            List<HIS_SERVICE_GROUP> result = new List<HIS_SERVICE_GROUP>();
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

        public HIS_SERVICE_GROUP GetById(long id, HisServiceGroupSO search)
        {
            HIS_SERVICE_GROUP result = null;
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
