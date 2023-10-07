using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentCare
{
    partial class HisAccidentCareGet : BusinessBase
    {
        internal HisAccidentCareGet()
            : base()
        {

        }

        internal HisAccidentCareGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_CARE> Get(HisAccidentCareFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentCareDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_CARE GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentCareFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_CARE GetById(long id, HisAccidentCareFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentCareDAO.GetById(id, filter.Query());
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
