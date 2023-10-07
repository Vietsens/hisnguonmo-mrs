using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisUserInvoiceBook
{
    public partial class HisUserInvoiceBookDAO : EntityBase
    {
        private HisUserInvoiceBookGet GetWorker
        {
            get
            {
                return (HisUserInvoiceBookGet)Worker.Get<HisUserInvoiceBookGet>();
            }
        }
        public List<HIS_USER_INVOICE_BOOK> Get(HisUserInvoiceBookSO search, CommonParam param)
        {
            List<HIS_USER_INVOICE_BOOK> result = new List<HIS_USER_INVOICE_BOOK>();
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

        public HIS_USER_INVOICE_BOOK GetById(long id, HisUserInvoiceBookSO search)
        {
            HIS_USER_INVOICE_BOOK result = null;
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
