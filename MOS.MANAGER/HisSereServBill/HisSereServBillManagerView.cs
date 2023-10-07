using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServBill
{
    public partial class HisSereServBillManager : BusinessBase
    {
        
        public List<V_HIS_SERE_SERV_BILL> GetView(HisSereServBillViewFilterQuery filter)
        {
            List<V_HIS_SERE_SERV_BILL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERE_SERV_BILL> resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetView(filter);
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

        
        public V_HIS_SERE_SERV_BILL GetViewById(long data)
        {
            V_HIS_SERE_SERV_BILL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SERE_SERV_BILL resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetViewById(data);
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

        
        public V_HIS_SERE_SERV_BILL GetViewById(long data, HisSereServBillViewFilterQuery filter)
        {
            V_HIS_SERE_SERV_BILL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SERE_SERV_BILL resultData = null;
                if (valid)
                {
                    resultData = new HisSereServBillGet(param).GetViewById(data, filter);
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
