using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockExty
{
    public partial class HisMediStockExtyDAO : EntityBase
    {
        private HisMediStockExtyGet GetWorker
        {
            get
            {
                return (HisMediStockExtyGet)Worker.Get<HisMediStockExtyGet>();
            }
        }

        public List<HIS_MEDI_STOCK_EXTY> Get(HisMediStockExtySO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK_EXTY> result = new List<HIS_MEDI_STOCK_EXTY>();
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

        public HIS_MEDI_STOCK_EXTY GetById(long id, HisMediStockExtySO search)
        {
            HIS_MEDI_STOCK_EXTY result = null;
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
