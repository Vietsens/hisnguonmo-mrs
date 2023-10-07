using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockPeriod
{
    public partial class HisMediStockPeriodManager : BusinessBase
    {
        public HisMediStockPeriodManager()
            : base()
        {

        }

        public HisMediStockPeriodManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_MEDI_STOCK_PERIOD> Get(HisMediStockPeriodFilterQuery filter)
        {
            List<HIS_MEDI_STOCK_PERIOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEDI_STOCK_PERIOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).Get(filter);
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

        
        public HIS_MEDI_STOCK_PERIOD GetById(long data)
        {
            HIS_MEDI_STOCK_PERIOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK_PERIOD resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetById(data);
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

        
        public HIS_MEDI_STOCK_PERIOD GetById(long data, HisMediStockPeriodFilterQuery filter)
        {
            HIS_MEDI_STOCK_PERIOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEDI_STOCK_PERIOD resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetById(data, filter);
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

        
        public HIS_MEDI_STOCK_PERIOD GetTheLast(long data)
        {
            HIS_MEDI_STOCK_PERIOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEDI_STOCK_PERIOD resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetTheLast(data);
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

        
        public List<HIS_MEDI_STOCK_PERIOD> GetByMediStockId(long data)
        {
            List<HIS_MEDI_STOCK_PERIOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_PERIOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetByMediStockId(data);
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

        
        public List<HIS_MEDI_STOCK_PERIOD> GetByPreviousId(long data)
        {
            List<HIS_MEDI_STOCK_PERIOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEDI_STOCK_PERIOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetByPreviousId(data);
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
