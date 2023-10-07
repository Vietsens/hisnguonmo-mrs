using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInvoiceBook
{
    public partial class HisInvoiceBookDAO : EntityBase
    {
        private HisInvoiceBookGet GetWorker
        {
            get
            {
                return (HisInvoiceBookGet)Worker.Get<HisInvoiceBookGet>();
            }
        }
        public List<HIS_INVOICE_BOOK> Get(HisInvoiceBookSO search, CommonParam param)
        {
            List<HIS_INVOICE_BOOK> result = new List<HIS_INVOICE_BOOK>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_INVOICE_BOOK GetById(long id, HisInvoiceBookSO search)
        {
            HIS_INVOICE_BOOK result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
