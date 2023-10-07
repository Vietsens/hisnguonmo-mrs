using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisUserInvoiceBook
{
    partial class HisUserInvoiceBookGet : BusinessBase
    {
        internal HisUserInvoiceBookGet()
            : base()
        {

        }

        internal HisUserInvoiceBookGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_USER_INVOICE_BOOK> Get(HisUserInvoiceBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserInvoiceBookDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_USER_INVOICE_BOOK GetById(long id)
        {
            try
            {
                return GetById(id, new HisUserInvoiceBookFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_USER_INVOICE_BOOK GetById(long id, HisUserInvoiceBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserInvoiceBookDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_USER_INVOICE_BOOK> GetView(HisUserInvoiceBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserInvoiceBookDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_USER_INVOICE_BOOK GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisUserInvoiceBookViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_USER_INVOICE_BOOK GetViewById(long id, HisUserInvoiceBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisUserInvoiceBookDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_USER_INVOICE_BOOK> GetByInvoiceBookId(long invoiceBookId)
        {
            try
            {
                HisUserInvoiceBookFilterQuery filter = new HisUserInvoiceBookFilterQuery();
                filter.INVOICE_BOOK_ID = invoiceBookId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_USER_INVOICE_BOOK> GetByInvoiceBookIdAndLoginName(long invoiceBookId, string loginName)
        {
            try
            {
                HisUserInvoiceBookFilterQuery filter = new HisUserInvoiceBookFilterQuery();
                filter.INVOICE_BOOK_ID = invoiceBookId;
                filter.LOGINNAME = loginName;
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
