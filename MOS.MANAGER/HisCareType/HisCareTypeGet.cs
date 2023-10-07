using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareType
{
    partial class HisCareTypeGet : BusinessBase
    {
        internal HisCareTypeGet()
            : base()
        {

        }

        internal HisCareTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARE_TYPE> Get(HisCareTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisCareTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_TYPE GetById(long id, HisCareTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisCareTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARE_TYPE GetByCode(string code, HisCareTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareTypeDAO.GetByCode(code, filter.Query());
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
