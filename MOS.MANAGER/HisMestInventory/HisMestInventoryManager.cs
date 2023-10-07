using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestInventory
{
    public partial class HisMestInventoryManager : BusinessBase
    {
        public HisMestInventoryManager()
            : base()
        {

        }

        public HisMestInventoryManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_INVENTORY> Get(HisMestInventoryFilterQuery filter)
        {
             List<HIS_MEST_INVENTORY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_INVENTORY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestInventoryGet(param).Get(filter);
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

        
        public  HIS_MEST_INVENTORY GetById(long data)
        {
             HIS_MEST_INVENTORY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_INVENTORY resultData = null;
                if (valid)
                {
                    resultData = new HisMestInventoryGet(param).GetById(data);
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

        
        public  HIS_MEST_INVENTORY GetById(long data, HisMestInventoryFilterQuery filter)
        {
             HIS_MEST_INVENTORY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_INVENTORY resultData = null;
                if (valid)
                {
                    resultData = new HisMestInventoryGet(param).GetById(data, filter);
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

        
        public  HIS_MEST_INVENTORY GetByMediStockPeriodId(long data)
        {
             HIS_MEST_INVENTORY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_INVENTORY resultData = null;
                if (valid)
                {
                    resultData = new HisMestInventoryGet(param).GetByMediStockPeriodId(data);
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
