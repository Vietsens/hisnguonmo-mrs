using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMate
{
    public partial class HisMestPeriodMateManager : BusinessBase
    {
        public HisMestPeriodMateManager()
            : base()
        {

        }

        public HisMestPeriodMateManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_MATE> Get(HisMestPeriodMateFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_MATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_MATE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_MATE GetById(long data)
        {
             HIS_MEST_PERIOD_MATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_MATE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_MATE GetById(long data, HisMestPeriodMateFilterQuery filter)
        {
             HIS_MEST_PERIOD_MATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_MATE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PERIOD_MATE> GetByMaterialId(long data)
        {
             List<HIS_MEST_PERIOD_MATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MATE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetByMaterialId(data);
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

        
        public  List<HIS_MEST_PERIOD_MATE> GetByMediStockPeriodId(long data)
        {
             List<HIS_MEST_PERIOD_MATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MATE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetByMediStockPeriodId(data);
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
