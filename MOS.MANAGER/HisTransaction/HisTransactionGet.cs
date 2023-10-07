using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTransaction
{
    partial class HisTransactionGet : GetBase
    {
        internal HisTransactionGet()
            : base()
        {

        }

        internal HisTransactionGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TRANSACTION> Get(HisTransactionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetTotal(HisTransactionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_TRANSACTION> GetView(HisTransactionViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION GetById(long id)
        {
            try
            {
                return GetById(id, new HisTransactionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION GetById(long id, HisTransactionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisTransactionViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION GetViewById(long id, HisTransactionViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisTransactionFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TRANSACTION GetByCode(string code, HisTransactionFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisTransactionViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_TRANSACTION GetViewByCode(string code, HisTransactionViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTransactionDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetByPayFormId(long id)
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.PAY_FORM_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetByTransactionTypeId(long id)
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.TRANSACTION_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetByTreatmentId(long id)
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.TREATMENT_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetByAccountBookId(long id)
        {
            try
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.ACCOUNT_BOOK_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_TRANSACTION> GetByCashierRoomId(long id)
        {
            HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
            filter.CASHIER_ROOM_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TRANSACTION> GetByCashoutId(long id)
        {
            HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
            filter.CASHOUT_ID = id;
            return this.Get(filter);
        }

        internal List<HIS_TRANSACTION> GetByIds(List<long> ids)
        {
            if (IsNotNullOrEmpty(ids))
            {
                HisTransactionFilterQuery filter = new HisTransactionFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            return null;
        }
    }
}
