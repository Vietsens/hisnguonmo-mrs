using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoice
{
    public partial class HisInvoiceManager : BusinessBase
    {
        
        public List<V_HIS_INVOICE> GetView(HisInvoiceViewFilterQuery filter)
        {
            List<V_HIS_INVOICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_INVOICE> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetView(filter);
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

        
        public V_HIS_INVOICE GetViewById(long data)
        {
            V_HIS_INVOICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_INVOICE resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetViewById(data);
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

        
        public V_HIS_INVOICE GetViewById(long data, HisInvoiceViewFilterQuery filter)
        {
            V_HIS_INVOICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_INVOICE resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetViewById(data, filter);
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
