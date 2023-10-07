using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipTempUser
{
    partial class HisEkipTempUserGet : BusinessBase
    {
        internal HisEkipTempUserGet()
            : base()
        {

        }

        internal HisEkipTempUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EKIP_TEMP_USER> Get(HisEkipTempUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipTempUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_TEMP_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisEkipTempUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_TEMP_USER GetById(long id, HisEkipTempUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipTempUserDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EKIP_TEMP_USER> GetByEkipTempId(long ekipTempId)
        {
            HisEkipTempUserFilterQuery filter = new HisEkipTempUserFilterQuery();
            filter.EKIP_TEMP_ID = ekipTempId;
            return this.Get(filter);
        }
    }
}
