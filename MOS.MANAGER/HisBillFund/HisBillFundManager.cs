using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    public partial class HisBillFundManager : BusinessBase
    {
        public HisBillFundManager()
            : base()
        {

        }

        public HisBillFundManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BILL_FUND> Get(HisBillFundFilterQuery filter)
        {
             List<HIS_BILL_FUND> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BILL_FUND> resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).Get(filter);
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

        
        public  HIS_BILL_FUND GetById(long data)
        {
             HIS_BILL_FUND result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BILL_FUND resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetById(data);
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

        
        public  HIS_BILL_FUND GetById(long data, HisBillFundFilterQuery filter)
        {
             HIS_BILL_FUND result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BILL_FUND resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetById(data, filter);
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

        
        public  List<HIS_BILL_FUND> GetByBillId(long data)
        {
             List<HIS_BILL_FUND> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_BILL_FUND> resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetByBillId(data);
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
