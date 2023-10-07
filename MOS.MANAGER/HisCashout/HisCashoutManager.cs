using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCashout
{
    public partial class HisCashoutManager : BusinessBase
    {
        public HisCashoutManager()
            : base()
        {

        }

        public HisCashoutManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_CASHOUT> Get(HisCashoutFilterQuery filter)
        {
            List<HIS_CASHOUT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CASHOUT> resultData = null;
                if (valid)
                {
                    resultData = new HisCashoutGet(param).Get(filter);
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

        
        public HIS_CASHOUT GetById(long data)
        {
            HIS_CASHOUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CASHOUT resultData = null;
                if (valid)
                {
                    resultData = new HisCashoutGet(param).GetById(data);
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

        
        public HIS_CASHOUT GetById(long data, HisCashoutFilterQuery filter)
        {
            HIS_CASHOUT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CASHOUT resultData = null;
                if (valid)
                {
                    resultData = new HisCashoutGet(param).GetById(data, filter);
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
