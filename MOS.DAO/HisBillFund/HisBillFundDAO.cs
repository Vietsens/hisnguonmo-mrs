using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBillFund
{
    public partial class HisBillFundDAO : EntityBase
    {
        private HisBillFundGet GetWorker
        {
            get
            {
                return (HisBillFundGet)Worker.Get<HisBillFundGet>();
            }
        }
        public List<HIS_BILL_FUND> Get(HisBillFundSO search, CommonParam param)
        {
            List<HIS_BILL_FUND> result = new List<HIS_BILL_FUND>();
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

        public HIS_BILL_FUND GetById(long id, HisBillFundSO search)
        {
            HIS_BILL_FUND result = null;
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
