using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MRS.Processor.Mrs00105
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

        
    }
}
