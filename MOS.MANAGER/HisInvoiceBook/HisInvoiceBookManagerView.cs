using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceBook
{
    public partial class HisInvoiceBookManager : BusinessBase
    {
        
        public List<V_HIS_INVOICE_BOOK> GetView(HisInvoiceBookViewFilterQuery filter)
        {
            List<V_HIS_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetView(filter);
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

        
        public V_HIS_INVOICE_BOOK GetViewById(long data)
        {
            V_HIS_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetViewById(data);
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

        
        public V_HIS_INVOICE_BOOK GetViewById(long data, HisInvoiceBookViewFilterQuery filter)
        {
            V_HIS_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_INVOICE_BOOK> GetViewBySymbolCodeAndTemplateCode(string symbolCode, string templateCode)
        {
            List<V_HIS_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(symbolCode);
                valid = valid && IsNotNull(templateCode);
                List<V_HIS_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetViewBySymbolCodeAndTemplateCode(symbolCode, templateCode);
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

        
        public V_HIS_INVOICE_BOOK GetViewByLinkId(long data)
        {
            V_HIS_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetViewByLinkId(data);
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
