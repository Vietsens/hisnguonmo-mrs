using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoicePrint
{
    partial class HisInvoicePrintGet : BusinessBase
    {
        internal HisInvoicePrintGet()
            : base()
        {

        }

        internal HisInvoicePrintGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INVOICE_PRINT> Get(HisInvoicePrintFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoicePrintDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_PRINT GetById(long id)
        {
            try
            {
                return GetById(id, new HisInvoicePrintFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_PRINT GetById(long id, HisInvoicePrintFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoicePrintDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
