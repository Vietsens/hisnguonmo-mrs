using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockMety
{
    public partial class HisMediStockMetyManager : BusinessBase
    {
        public HisMediStockMetyManager()
            : base()
        {

        }

        public HisMediStockMetyManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDI_STOCK_METY> Get(HisMediStockMetyFilterQuery filter)
        {
            List<HIS_MEDI_STOCK_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_STOCK_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).Get(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDI_STOCK_METY GetById(long data)
        {
            HIS_MEDI_STOCK_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public HIS_MEDI_STOCK_METY GetById(long data, HisMediStockMetyFilterQuery filter)
        {
            HIS_MEDI_STOCK_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_STOCK_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDI_STOCK_METY> GetByMediStockId(long data)
        {
            List<HIS_MEDI_STOCK_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetByMediStockId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public List<HIS_MEDI_STOCK_METY> GetByMedicineTypeId(long data)
        {
            List<HIS_MEDI_STOCK_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetByMedicineTypeId(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
