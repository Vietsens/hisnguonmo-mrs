using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockMety
{
    public partial class HisMediStockMetyDAO : EntityBase
    {
        private HisMediStockMetyGet GetWorker
        {
            get
            {
                return (HisMediStockMetyGet)Worker.Get<HisMediStockMetyGet>();
            }
        }
        public List<HIS_MEDI_STOCK_METY> Get(HisMediStockMetySO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK_METY> result = new List<HIS_MEDI_STOCK_METY>();
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

        public HIS_MEDI_STOCK_METY GetById(long id, HisMediStockMetySO search)
        {
            HIS_MEDI_STOCK_METY result = null;
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
