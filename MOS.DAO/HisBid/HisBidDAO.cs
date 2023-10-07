using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBid
{
    public partial class HisBidDAO : EntityBase
    {
        private HisBidGet GetWorker
        {
            get
            {
                return (HisBidGet)Worker.Get<HisBidGet>();
            }
        }
        public List<HIS_BID> Get(HisBidSO search, CommonParam param)
        {
            List<HIS_BID> result = new List<HIS_BID>();
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

        public HIS_BID GetById(long id, HisBidSO search)
        {
            HIS_BID result = null;
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
