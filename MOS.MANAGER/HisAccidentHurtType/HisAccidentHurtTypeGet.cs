using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurtType
{
    partial class HisAccidentHurtTypeGet : BusinessBase
    {
        internal HisAccidentHurtTypeGet()
            : base()
        {

        }

        internal HisAccidentHurtTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_HURT_TYPE> Get(HisAccidentHurtTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentHurtTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT_TYPE GetById(long id, HisAccidentHurtTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtTypeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT_TYPE GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisAccidentHurtTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HURT_TYPE GetByCode(string code, HisAccidentHurtTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtTypeDAO.GetByCode(code, filter.Query());
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
