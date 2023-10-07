using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisFund
{
    public partial class HisFundDAO : EntityBase
    {
        private HisFundGet GetWorker
        {
            get
            {
                return (HisFundGet)Worker.Get<HisFundGet>();
            }
        }
        public List<HIS_FUND> Get(HisFundSO search, CommonParam param)
        {
            List<HIS_FUND> result = new List<HIS_FUND>();
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

        public HIS_FUND GetById(long id, HisFundSO search)
        {
            HIS_FUND result = null;
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
