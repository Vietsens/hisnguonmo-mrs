using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    partial class HisBillFundGet : BusinessBase
    {
        internal HisBillFundGet()
            : base()
        {

        }

        internal HisBillFundGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BILL_FUND> Get(HisBillFundFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillFundDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BILL_FUND GetById(long id)
        {
            try
            {
                return GetById(id, new HisBillFundFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BILL_FUND GetById(long id, HisBillFundFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillFundDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_BILL_FUND> GetByBillId(long billId)
        {
            try
            {
                HisBillFundFilterQuery filter = new HisBillFundFilterQuery();
                filter.BILL_ID = billId;
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
