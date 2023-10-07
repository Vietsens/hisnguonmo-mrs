using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodBlty
{
    public partial class HisMestPeriodBltyManager : BusinessBase
    {
        public HisMestPeriodBltyManager()
            : base()
        {

        }

        public HisMestPeriodBltyManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PERIOD_BLTY> Get(HisMestPeriodBltyFilterQuery filter)
        {
             List<HIS_MEST_PERIOD_BLTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PERIOD_BLTY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBltyGet(param).Get(filter);
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

        
        public  HIS_MEST_PERIOD_BLTY GetById(long data)
        {
             HIS_MEST_PERIOD_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PERIOD_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBltyGet(param).GetById(data);
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

        
        public  HIS_MEST_PERIOD_BLTY GetById(long data, HisMestPeriodBltyFilterQuery filter)
        {
             HIS_MEST_PERIOD_BLTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PERIOD_BLTY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodBltyGet(param).GetById(data, filter);
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
