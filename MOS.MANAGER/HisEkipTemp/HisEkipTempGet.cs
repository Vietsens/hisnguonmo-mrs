using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTemp
{
    partial class HisEkipTempGet : BusinessBase
    {
        internal HisEkipTempGet()
            : base()
        {

        }

        internal HisEkipTempGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EKIP_TEMP> Get(HisEkipTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipTempDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_TEMP GetById(long id)
        {
            try
            {
                return GetById(id, new HisEkipTempFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_TEMP GetById(long id, HisEkipTempFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipTempDAO.GetById(id, filter.Query());
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
