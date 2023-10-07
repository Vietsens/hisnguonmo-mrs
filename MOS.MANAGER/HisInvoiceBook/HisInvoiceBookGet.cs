using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisInvoiceBook
{
    partial class HisInvoiceBookGet : BusinessBase
    {
        internal HisInvoiceBookGet()
            : base()
        {

        }

        internal HisInvoiceBookGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_INVOICE_BOOK> Get(HisInvoiceBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceBookDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_INVOICE_BOOK> GetView(HisInvoiceBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceBookDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_BOOK GetById(long id)
        {
            try
            {
                return GetById(id, new HisInvoiceBookFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_INVOICE_BOOK GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisInvoiceBookViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_INVOICE_BOOK GetById(long id, HisInvoiceBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceBookDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_INVOICE_BOOK GetViewById(long id, HisInvoiceBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisInvoiceBookDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_INVOICE_BOOK> GetBySymbolCodeAndTemplateCode(string symbolCode, string templateCode)
        {
            try
            {
                HisInvoiceBookFilterQuery filter = new HisInvoiceBookFilterQuery();
                filter.SYMBOL_CODE__EXACT = symbolCode;
                filter.TEMPLATE_CODE__EXACT = templateCode;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_INVOICE_BOOK> GetViewBySymbolCodeAndTemplateCode(string symbolCode, string templateCode)
        {
            HisInvoiceBookViewFilterQuery filter = new HisInvoiceBookViewFilterQuery();
            filter.SYMBOL_CODE__EXACT = symbolCode;
            filter.TEMPLATE_CODE__EXACT = templateCode;
            return this.GetView(filter);
        }

        internal V_HIS_INVOICE_BOOK GetViewByLinkId(long linkId)
        {
            HisInvoiceBookViewFilterQuery filter = new HisInvoiceBookViewFilterQuery();
            filter.LINK_ID = linkId;
            List<V_HIS_INVOICE_BOOK> data = this.GetView(filter);
            return IsNotNullOrEmpty(data) ? data[0] : null;
        }
    }
}
