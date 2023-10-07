using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestStt
{
    class HisExpMestSttGet : GetBase
    {
        internal HisExpMestSttGet()
            : base()
        {

        }

        internal HisExpMestSttGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXP_MEST_STT> Get(HisExpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestSttDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_STT GetById(long id)
        {
            try
            {
                return GetById(id, new HisExpMestSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_STT GetById(long id, HisExpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestSttDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_STT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisExpMestSttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXP_MEST_STT GetByCode(string code, HisExpMestSttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestSttDAO.GetByCode(code, filter.Query());
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
