using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockImty
{
    public partial class HisMediStockImtyDAO : EntityBase
    {
        private HisMediStockImtyGet GetWorker
        {
            get
            {
                return (HisMediStockImtyGet)Worker.Get<HisMediStockImtyGet>();
            }
        }

        public List<HIS_MEDI_STOCK_IMTY> Get(HisMediStockImtySO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK_IMTY> result = new List<HIS_MEDI_STOCK_IMTY>();
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

        public HIS_MEDI_STOCK_IMTY GetById(long id, HisMediStockImtySO search)
        {
            HIS_MEDI_STOCK_IMTY result = null;
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
