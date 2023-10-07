using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOweType
{
    partial class HisOweTypeGet : BusinessBase
    {
        internal HisOweTypeGet()
            : base()
        {

        }

        internal HisOweTypeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_OWE_TYPE> Get(HisOweTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisOweTypeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_OWE_TYPE GetById(long id)
        {
            try
            {
                return GetById(id, new HisOweTypeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_OWE_TYPE GetById(long id, HisOweTypeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisOweTypeDAO.GetById(id, filter.Query());
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
