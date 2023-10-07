using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceDetail
{
    public partial class HisInvoiceDetailManager : BusinessBase
    {
        public HisInvoiceDetailManager()
            : base()
        {

        }

        public HisInvoiceDetailManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_INVOICE_DETAIL> Get(HisInvoiceDetailFilterQuery filter)
        {
            List<HIS_INVOICE_DETAIL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INVOICE_DETAIL> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceDetailGet(param).Get(filter);
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

        
        public HIS_INVOICE_DETAIL GetById(long data)
        {
            HIS_INVOICE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INVOICE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceDetailGet(param).GetById(data);
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

        
        public HIS_INVOICE_DETAIL GetById(long data, HisInvoiceDetailFilterQuery filter)
        {
            HIS_INVOICE_DETAIL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INVOICE_DETAIL resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceDetailGet(param).GetById(data, filter);
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

        
        public List<HIS_INVOICE_DETAIL> GetByInvoiceId(long data)
        {
            List<HIS_INVOICE_DETAIL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_INVOICE_DETAIL> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceDetailGet(param).GetByInvoiceId(data);
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
