using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediStock
{
    public partial class HisMediStockDAO : EntityBase
    {
        public List<V_HIS_MEDI_STOCK> GetView(HisMediStockSO search, CommonParam param)
        {
            List<V_HIS_MEDI_STOCK> result = new List<V_HIS_MEDI_STOCK>();

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

        public HIS_MEDI_STOCK GetByCode(string code, HisMediStockSO search)
        {
            HIS_MEDI_STOCK result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_HIS_MEDI_STOCK GetViewById(long id, HisMediStockSO search)
        {
            V_HIS_MEDI_STOCK result = null;

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

        public V_HIS_MEDI_STOCK GetViewByCode(string code, HisMediStockSO search)
        {
            V_HIS_MEDI_STOCK result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_MEDI_STOCK> GetDicByCode(HisMediStockSO search, CommonParam param)
        {
            Dictionary<string, HIS_MEDI_STOCK> result = new Dictionary<string, HIS_MEDI_STOCK>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
