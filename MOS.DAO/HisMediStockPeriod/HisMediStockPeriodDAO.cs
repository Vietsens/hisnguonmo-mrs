using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockPeriod
{
    public partial class HisMediStockPeriodDAO : EntityBase
    {
        private HisMediStockPeriodGet GetWorker
        {
            get
            {
                return (HisMediStockPeriodGet)Worker.Get<HisMediStockPeriodGet>();
            }
        }
        public List<HIS_MEDI_STOCK_PERIOD> Get(HisMediStockPeriodSO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK_PERIOD> result = new List<HIS_MEDI_STOCK_PERIOD>();
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

        public HIS_MEDI_STOCK_PERIOD GetById(long id, HisMediStockPeriodSO search)
        {
            HIS_MEDI_STOCK_PERIOD result = null;
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
