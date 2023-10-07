using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisImpMestStt
{
    class HisImpMestSttGet : GetBase
    {
        internal HisImpMestSttGet()
            : base()
        {

        }

        internal HisImpMestSttGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_IMP_MEST_STT> Get(HisImpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestSttDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_STT GetById(long id)
        {
            try
            {
                return GetById(id, new HisImpMestSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_STT GetById(long id, HisImpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestSttDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_STT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisImpMestSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_IMP_MEST_STT GetByCode(string code, HisImpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisImpMestSttDAO.GetByCode(code, filter.Query());
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
