using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlood
{
    public partial class HisMestPeriodBloodManager : BusinessBase
    {
        public HisMestPeriodBloodManager()
            : base()
        {

        }

        public HisMestPeriodBloodManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_BLOOD> Get(HisMestPeriodBloodFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBloodGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_BLOOD GetById(long data)
        {
             HIS_MEST_PERIOD_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBloodGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_BLOOD GetById(long data, HisMestPeriodBloodFilterQuery filter)
        {
             HIS_MEST_PERIOD_BLOOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_BLOOD resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBloodGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PERIOD_BLOOD> GetByBloodId(long data)
        {
             List<HIS_MEST_PERIOD_BLOOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PERIOD_BLOOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBloodGet(param).GetByBloodId(data);
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
