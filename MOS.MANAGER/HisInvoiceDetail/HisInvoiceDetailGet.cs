using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceDetail
{
    partial class HisInvoiceDetailGet : BusinessBase
    {
        internal HisInvoiceDetailGet()
            : base()
        {

        }

        internal HisInvoiceDetailGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INVOICE_DETAIL> Get(HisInvoiceDetailFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDetailDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_DETAIL GetById(long id)
        {
            try
            {
                return GetById(id, new HisInvoiceDetailFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_DETAIL GetById(long id, HisInvoiceDetailFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDetailDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_INVOICE_DETAIL> GetByInvoiceId(long invoiceId)
        {
            try
            {
                HisInvoiceDetailFilterQuery filter = new HisInvoiceDetailFilterQuery();
                filter.INVOICE_ID = invoiceId;
                return this.Get(filter);
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
