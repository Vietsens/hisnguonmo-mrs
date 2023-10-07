using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHelmet
{
    partial class HisAccidentHelmetGet : BusinessBase
    {
        internal HisAccidentHelmetGet()
            : base()
        {

        }

        internal HisAccidentHelmetGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ACCIDENT_HELMET> Get(HisAccidentHelmetFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHelmetDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HELMET GetById(long id)
        {
            try
            {
                return GetById(id, new HisAccidentHelmetFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ACCIDENT_HELMET GetById(long id, HisAccidentHelmetFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHelmetDAO.GetById(id, filter.Query());
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
