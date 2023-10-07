using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMedi
{
    public partial class HisMestPeriodMediManager : BusinessBase
    {
        public HisMestPeriodMediManager()
            : base()
        {

        }

        public HisMestPeriodMediManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_MEDI> Get(HisMestPeriodMediFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_MEDI> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_MEDI> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_MEDI GetById(long data)
        {
             HIS_MEST_PERIOD_MEDI result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_MEDI resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_MEDI GetById(long data, HisMestPeriodMediFilterQuery filter)
        {
             HIS_MEST_PERIOD_MEDI result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_MEDI resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PERIOD_MEDI> GetByMedicineId(long data)
        {
             List<HIS_MEST_PERIOD_MEDI> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MEDI> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetByMedicineId(data);
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

        
        public  List<HIS_MEST_PERIOD_MEDI> GetByMediStockPeriodId(long data)
        {
             List<HIS_MEST_PERIOD_MEDI> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_MEDI> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetByMediStockPeriodId(data);
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
