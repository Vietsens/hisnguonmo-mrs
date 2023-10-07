using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInvoiceDetail
{
    public partial class HisInvoiceDetailDAO : EntityBase
    {
        private HisInvoiceDetailGet GetWorker
        {
            get
            {
                return (HisInvoiceDetailGet)Worker.Get<HisInvoiceDetailGet>();
            }
        }
        public List<HIS_INVOICE_DETAIL> Get(HisInvoiceDetailSO search, CommonParam param)
        {
            List<HIS_INVOICE_DETAIL> result = new List<HIS_INVOICE_DETAIL>();
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

        public HIS_INVOICE_DETAIL GetById(long id, HisInvoiceDetailSO search)
        {
            HIS_INVOICE_DETAIL result = null;
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
