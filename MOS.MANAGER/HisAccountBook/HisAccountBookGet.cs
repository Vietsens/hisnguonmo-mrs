using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccountBook
{
    partial class HisAccountBookGet : GetBase
    {
        internal HisAccountBookGet()
            : base()
        {

        }

        internal HisAccountBookGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCOUNT_BOOK> Get(HisAccountBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_ACCOUNT_BOOK> GetView(HisAccountBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCOUNT_BOOK GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccountBookFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCOUNT_BOOK GetById(long id, HisAccountBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCOUNT_BOOK GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAccountBookViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCOUNT_BOOK GetViewById(long id, HisAccountBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCOUNT_BOOK GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccountBookFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCOUNT_BOOK GetByCode(string code, HisAccountBookFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCOUNT_BOOK GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisAccountBookViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCOUNT_BOOK GetViewByCode(string code, HisAccountBookViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccountBookDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ACCOUNT_BOOK> GetByCodes(List<string> accountBookCodes)
        {
            List<HIS_ACCOUNT_BOOK> result = new List<HIS_ACCOUNT_BOOK>();
            try
            {
                foreach (var item in accountBookCodes)
                {
                    result.Add(GetByCode(item));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
            return result;
        }
    }
}
