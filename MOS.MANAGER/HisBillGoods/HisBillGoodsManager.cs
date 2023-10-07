using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillGoods
{
    public partial class HisBillGoodsManager : BusinessBase
    {
        public HisBillGoodsManager()
            : base()
        {

        }

        public HisBillGoodsManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BILL_GOODS> Get(HisBillGoodsFilterQuery filter)
        {
             List<HIS_BILL_GOODS> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BILL_GOODS> resultData = null;
                if (valid)
                {
                    resultData = new HisBillGoodsGet(param).Get(filter);
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

        
        public  HIS_BILL_GOODS GetById(long data)
        {
             HIS_BILL_GOODS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BILL_GOODS resultData = null;
                if (valid)
                {
                    resultData = new HisBillGoodsGet(param).GetById(data);
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

        
        public  HIS_BILL_GOODS GetById(long data, HisBillGoodsFilterQuery filter)
        {
             HIS_BILL_GOODS result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BILL_GOODS resultData = null;
                if (valid)
                {
                    resultData = new HisBillGoodsGet(param).GetById(data, filter);
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
