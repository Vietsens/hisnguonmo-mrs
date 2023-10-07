using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTextLib
{
    class HisTextLibGet : GetBase
    {
        internal HisTextLibGet()
            : base()
        {

        }

        internal HisTextLibGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_TEXT_LIB> Get(HisTextLibFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTextLibDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEXT_LIB GetById(long id)
        {
            try
            {
                return GetById(id, new HisTextLibFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_TEXT_LIB GetById(long id, HisTextLibFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisTextLibDAO.GetById(id, filter.Query());
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
