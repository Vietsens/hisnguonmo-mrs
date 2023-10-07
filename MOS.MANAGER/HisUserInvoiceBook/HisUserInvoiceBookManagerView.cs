using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserInvoiceBook
{
    public partial class HisUserInvoiceBookManager : BusinessBase
    {
        
        public List<V_HIS_USER_INVOICE_BOOK> GetView(HisUserInvoiceBookViewFilterQuery filter)
        {
            List<V_HIS_USER_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_USER_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).GetView(filter);
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

        
        public V_HIS_USER_INVOICE_BOOK GetViewById(long data)
        {
            V_HIS_USER_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_USER_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).GetViewById(data);
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

        
        public V_HIS_USER_INVOICE_BOOK GetViewById(long data, HisUserInvoiceBookViewFilterQuery filter)
        {
            V_HIS_USER_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_USER_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).GetViewById(data, filter);
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
