using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEkipUser
{
    partial class HisEkipUserGet : BusinessBase
    {
        internal HisEkipUserGet()
            : base()
        {

        }

        internal HisEkipUserGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EKIP_USER> Get(HisEkipUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipUserDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_USER GetById(long id)
        {
            try
            {
                return GetById(id, new HisEkipUserFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EKIP_USER GetById(long id, HisEkipUserFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEkipUserDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EKIP_USER> GetByEkipId(long ekipId)
        {
            HisEkipUserFilterQuery filter = new HisEkipUserFilterQuery();
            filter.EKIP_ID = ekipId;
            return this.Get(filter);
        }
    }
}
