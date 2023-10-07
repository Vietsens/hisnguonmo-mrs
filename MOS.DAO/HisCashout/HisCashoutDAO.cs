using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCashout
{
    public partial class HisCashoutDAO : EntityBase
    {
        private HisCashoutGet GetWorker
        {
            get
            {
                return (HisCashoutGet)Worker.Get<HisCashoutGet>();
            }
        }
        public List<HIS_CASHOUT> Get(HisCashoutSO search, CommonParam param)
        {
            List<HIS_CASHOUT> result = new List<HIS_CASHOUT>();
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

        public HIS_CASHOUT GetById(long id, HisCashoutSO search)
        {
            HIS_CASHOUT result = null;
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
