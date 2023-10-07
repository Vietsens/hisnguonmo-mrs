using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentLocation
{
    partial class HisAccidentLocationGet : BusinessBase
    {
        internal HisAccidentLocationGet()
            : base()
        {

        }

        internal HisAccidentLocationGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_LOCATION> Get(HisAccidentLocationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentLocationDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_LOCATION GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentLocationFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_LOCATION GetById(long id, HisAccidentLocationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentLocationDAO.GetById(id, filter.Query());
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
