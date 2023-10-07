using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockMaty
{
    public partial class HisMediStockMatyDAO : EntityBase
    {
        private HisMediStockMatyGet GetWorker
        {
            get
            {
                return (HisMediStockMatyGet)Worker.Get<HisMediStockMatyGet>();
            }
        }
        public List<HIS_MEDI_STOCK_MATY> Get(HisMediStockMatySO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK_MATY> result = new List<HIS_MEDI_STOCK_MATY>();
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

        public HIS_MEDI_STOCK_MATY GetById(long id, HisMediStockMatySO search)
        {
            HIS_MEDI_STOCK_MATY result = null;
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
