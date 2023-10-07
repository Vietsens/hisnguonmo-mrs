using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCareer
{
    public class HisCareerGet : GetBase
    {
        internal HisCareerGet()
            : base()
        {

        }

        internal HisCareerGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CAREER> Get(HisCareerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareerDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CAREER GetById(long id)
        {
            try
            {
                return GetById(id, new HisCareerFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CAREER GetById(long id, HisCareerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareerDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CAREER GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisCareerFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CAREER GetByCode(string code, HisCareerFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCareerDAO.GetByCode(code, filter.Query());
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
