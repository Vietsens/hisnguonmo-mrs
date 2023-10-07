using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisInvoice
{
    public partial class HisInvoiceDAO : EntityBase
    {
        private HisInvoiceGet GetWorker
        {
            get
            {
                return (HisInvoiceGet)Worker.Get<HisInvoiceGet>();
            }
        }
        public List<HIS_INVOICE> Get(HisInvoiceSO search, CommonParam param)
        {
            List<HIS_INVOICE> result = new List<HIS_INVOICE>();
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

        public HIS_INVOICE GetById(long id, HisInvoiceSO search)
        {
            HIS_INVOICE result = null;
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
