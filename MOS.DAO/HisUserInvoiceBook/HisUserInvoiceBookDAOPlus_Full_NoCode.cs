using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisUserInvoiceBook
{
    public partial class HisUserInvoiceBookDAO : EntityBase
    {
        public List<V_HIS_USER_INVOICE_BOOK> GetView(HisUserInvoiceBookSO search, CommonParam param)
        {
            List<V_HIS_USER_INVOICE_BOOK> result = new List<V_HIS_USER_INVOICE_BOOK>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_USER_INVOICE_BOOK GetViewById(long id, HisUserInvoiceBookSO search)
        {
            V_HIS_USER_INVOICE_BOOK result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
