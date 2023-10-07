using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStockExty
{
    public partial class HisMediStockExtyDAO : EntityBase
    {
        public List<V_HIS_MEDI_STOCK_EXTY> GetView(HisMediStockExtySO search, CommonParam param)
        {
            List<V_HIS_MEDI_STOCK_EXTY> result = new List<V_HIS_MEDI_STOCK_EXTY>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public V_HIS_MEDI_STOCK_EXTY GetViewById(long id, HisMediStockExtySO search)
        {
            V_HIS_MEDI_STOCK_EXTY result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
