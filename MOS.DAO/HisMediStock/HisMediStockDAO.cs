using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStock
{
    public partial class HisMediStockDAO : EntityBase
    {
        private HisMediStockGet GetWorker
        {
            get
            {
                return (HisMediStockGet)Worker.Get<HisMediStockGet>();
            }
        }
        public List<HIS_MEDI_STOCK> Get(HisMediStockSO search, CommonParam param)
        {
            List<HIS_MEDI_STOCK> result = new List<HIS_MEDI_STOCK>();
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

        public HIS_MEDI_STOCK GetById(long id, HisMediStockSO search)
        {
            HIS_MEDI_STOCK result = null;
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
