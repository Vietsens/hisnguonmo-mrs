using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoice
{
    public partial class HisInvoiceManager : BusinessBase
    {
        public HisInvoiceManager()
            : base()
        {

        }

        public HisInvoiceManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_INVOICE> Get(HisInvoiceFilterQuery filter)
        {
            List<HIS_INVOICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_INVOICE> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).Get(filter);
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

        
        public HIS_INVOICE GetById(long data)
        {
            HIS_INVOICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_INVOICE resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetById(data);
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

        
        public HIS_INVOICE GetById(long data, HisInvoiceFilterQuery filter)
        {
            HIS_INVOICE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_INVOICE resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetById(data, filter);
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

        
        public List<HIS_INVOICE> GetByInvoiceBookId(long data)
        {
             List<HIS_INVOICE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_INVOICE> resultData = null;
                if (valid)
                {
                    resultData = new HisInvoiceGet(param).GetByInvoiceBookId(data);
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
