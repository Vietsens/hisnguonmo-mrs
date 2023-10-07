using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidType
{
    public partial class HisBidTypeDAO : EntityBase
    {
        private HisBidTypeGet GetWorker
        {
            get
            {
                return (HisBidTypeGet)Worker.Get<HisBidTypeGet>();
            }
        }
        public List<HIS_BID_TYPE> Get(HisBidTypeSO search, CommonParam param)
        {
            List<HIS_BID_TYPE> result = new List<HIS_BID_TYPE>();
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

        public HIS_BID_TYPE GetById(long id, HisBidTypeSO search)
        {
            HIS_BID_TYPE result = null;
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
