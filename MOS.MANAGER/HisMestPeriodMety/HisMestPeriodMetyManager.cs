using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMety
{
    public partial class HisMestPeriodMetyManager : BusinessBase
    {
        public HisMestPeriodMetyManager()
            : base()
        {

        }

        public HisMestPeriodMetyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_METY> Get(HisMestPeriodMetyFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_METY GetById(long data)
        {
             HIS_MEST_PERIOD_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_METY GetById(long data, HisMestPeriodMetyFilterQuery filter)
        {
             HIS_MEST_PERIOD_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PERIOD_METY> GetByMedicineTypeId(long data)
        {
             List<HIS_MEST_PERIOD_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetByMedicineTypeId(data);
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

        
        public  List<HIS_MEST_PERIOD_METY> GetByMediStockPeriodId(long data)
        {
             List<HIS_MEST_PERIOD_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetByMediStockPeriodId(data);
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
