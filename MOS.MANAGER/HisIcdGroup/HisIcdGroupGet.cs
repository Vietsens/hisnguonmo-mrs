using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdGroup
{
    class HisIcdGroupGet : GetBase
    {
        internal HisIcdGroupGet()
            : base()
        {

        }

        internal HisIcdGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ICD_GROUP> Get(HisIcdGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisIcdGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_GROUP GetById(long id, HisIcdGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdGroupDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_GROUP GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisIcdGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_GROUP GetByCode(string code, HisIcdGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdGroupDAO.GetByCode(code, filter.Query());
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
