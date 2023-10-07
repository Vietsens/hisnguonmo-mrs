using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserInvoiceBook
{
    public partial class HisUserInvoiceBookManager : BusinessBase
    {
        public HisUserInvoiceBookManager()
            : base()
        {

        }

        public HisUserInvoiceBookManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_USER_INVOICE_BOOK> Get(HisUserInvoiceBookFilterQuery filter)
        {
             List<HIS_USER_INVOICE_BOOK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_USER_INVOICE_BOOK> resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).Get(filter);
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

        
        public  HIS_USER_INVOICE_BOOK GetById(long data)
        {
             HIS_USER_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_USER_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).GetById(data);
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

        
        public  HIS_USER_INVOICE_BOOK GetById(long data, HisUserInvoiceBookFilterQuery filter)
        {
             HIS_USER_INVOICE_BOOK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_USER_INVOICE_BOOK resultData = null;
                if (valid)
                {
                    resultData = new HisUserInvoiceBookGet(param).GetById(data, filter);
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
