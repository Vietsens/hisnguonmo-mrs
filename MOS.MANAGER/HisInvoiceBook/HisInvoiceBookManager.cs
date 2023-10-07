using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceBook
{
    public partial class HisInvoiceBookManager : BusinessBase
    {
        public HisInvoiceBookManager()
            : base()
        {

        }

        public HisInvoiceBookManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_INVOICE_BOOK> Get(HisInvoiceBookFilterQuery filter)
        {
             List<HIS_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).Get(filter);
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

        
        public  HIS_INVOICE_BOOK GetById(long data)
        {
             HIS_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetById(data);
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

        
        public  HIS_INVOICE_BOOK GetById(long data, HisInvoiceBookFilterQuery filter)
        {
             HIS_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetById(data, filter);
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

        
        public  List<HIS_INVOICE_BOOK> GetBySymbolCodeAndTemplateCode(string symbolCode, string templateCode)
        {
             List<HIS_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(symbolCode);
                valid = valid && IsNotNull(templateCode);
                List<HIS_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceBookGet(param).GetBySymbolCodeAndTemplateCode(symbolCode, templateCode);
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
