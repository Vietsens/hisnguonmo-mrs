using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInvoicePrint
{
    public partial class HisInvoicePrintDAO : EntityBase
    {
        private HisInvoicePrintGet GetWorker
        {
            get
            {
                return (HisInvoicePrintGet)Worker.Get<HisInvoicePrintGet>();
            }
        }
        public List<HIS_INVOICE_PRINT> Get(HisInvoicePrintSO search, CommonParam param)
        {
            List<HIS_INVOICE_PRINT> result = new List<HIS_INVOICE_PRINT>();
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

        public HIS_INVOICE_PRINT GetById(long id, HisInvoicePrintSO search)
        {
            HIS_INVOICE_PRINT result = null;
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
