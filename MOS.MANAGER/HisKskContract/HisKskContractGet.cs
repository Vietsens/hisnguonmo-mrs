using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisKskContract
{
    partial class HisKskContractGet : BusinessBase
    {
        internal HisKskContractGet()
            : base()
        {

        }

        internal HisKskContractGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_KSK_CONTRACT> Get(HisKskContractFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisKskContractDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_KSK_CONTRACT GetById(long id)
        {
            try
            {
                return GetById(id, new HisKskContractFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_KSK_CONTRACT> GetByIds(List<long> ids)
        {
            try
            {
                if (IsNotNullOrEmpty(ids))
                {
                    HisKskContractFilterQuery filter = new HisKskContractFilterQuery();
                    filter.IDs = ids;
                    return this.Get(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_KSK_CONTRACT GetById(long id, HisKskContractFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisKskContractDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_KSK_CONTRACT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisKskContractFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_KSK_CONTRACT GetByCode(string code, HisKskContractFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisKskContractDAO.GetByCode(code, filter.Query());
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
