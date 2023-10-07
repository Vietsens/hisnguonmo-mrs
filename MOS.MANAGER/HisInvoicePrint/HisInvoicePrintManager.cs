using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoicePrint
{
    public partial class HisInvoicePrintManager : BusinessBase
    {
        public HisInvoicePrintManager()
            : base()
        {

        }

        public HisInvoicePrintManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_INVOICE_PRINT> Get(HisInvoicePrintFilterQuery filter)
        {
             List<HIS_INVOICE_PRINT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INVOICE_PRINT> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoicePrintGet(param).Get(filter);
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

        
        public  HIS_INVOICE_PRINT GetById(long data)
        {
             HIS_INVOICE_PRINT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INVOICE_PRINT resultData = null;
                if (valid)
                {
                    resultData = new HisInvoicePrintGet(param).GetById(data);
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

        
        public  HIS_INVOICE_PRINT GetById(long data, HisInvoicePrintFilterQuery filter)
        {
             HIS_INVOICE_PRINT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INVOICE_PRINT resultData = null;
                if (valid)
                {
                    resultData = new HisInvoicePrintGet(param).GetById(data, filter);
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
