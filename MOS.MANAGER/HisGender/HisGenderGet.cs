using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisGender
{
    class HisGenderGet : GetBase
    {
        internal HisGenderGet()
            : base()
        {

        }

        internal HisGenderGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_GENDER> Get(HisGenderFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisGenderDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_GENDER GetById(long id)
        {
            try
            {
                return GetById(id, new HisGenderFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_GENDER GetById(long id, HisGenderFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisGenderDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_GENDER GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisGenderFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_GENDER GetByCode(string code, HisGenderFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisGenderDAO.GetByCode(code, filter.Query());
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
