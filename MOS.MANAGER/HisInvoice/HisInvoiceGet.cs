using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoice
{
    partial class HisInvoiceGet : BusinessBase
    {
        internal HisInvoiceGet()
            : base()
        {

        }

        internal HisInvoiceGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INVOICE> Get(HisInvoiceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE GetById(long id)
        {
            try
            {
                return GetById(id, new HisInvoiceFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE GetById(long id, HisInvoiceFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_INVOICE> GetView(HisInvoiceViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_INVOICE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisInvoiceViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_INVOICE GetViewById(long id, HisInvoiceViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_INVOICE> GetByInvoiceBookId(long invoiceBookId)
        {
            try
            {
                HisInvoiceFilterQuery filter = new HisInvoiceFilterQuery();
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
    }
}
