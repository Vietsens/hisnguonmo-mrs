using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMaty
{
    public partial class HisMestPeriodMatyManager : BusinessBase
    {
        public HisMestPeriodMatyManager()
            : base()
        {

        }

        public HisMestPeriodMatyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_MATY> Get(HisMestPeriodMatyFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_MATY GetById(long data)
        {
             HIS_MEST_PERIOD_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_MATY GetById(long data, HisMestPeriodMatyFilterQuery filter)
        {
             HIS_MEST_PERIOD_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PERIOD_MATY> GetByMaterialTypeId(long data)
        {
             List<HIS_MEST_PERIOD_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetByMaterialTypeId(data);
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

        
        public  List<HIS_MEST_PERIOD_MATY> GetByMediStockPeriodId(long data)
        {
             List<HIS_MEST_PERIOD_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMatyGet(param).GetByMediStockPeriodId(data);
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
