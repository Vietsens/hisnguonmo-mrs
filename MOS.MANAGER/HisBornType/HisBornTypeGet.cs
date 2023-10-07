using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBornType
{
    partial class HisBornTypeGet : BusinessBase
    {
        internal HisBornTypeGet()
            : base()
        {

        }

        internal HisBornTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BORN_TYPE> Get(HisBornTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisBornTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BORN_TYPE GetById(long id, HisBornTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBornTypeDAO.GetById(id, filter.Query());
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
