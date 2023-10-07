using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediOrg
{
    partial class HisMediOrgGet : BusinessBase
    {
        internal HisMediOrgGet()
            : base()
        {

        }

        internal HisMediOrgGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_ORG> Get(HisMediOrgFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediOrgDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_ORG GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediOrgFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_ORG GetById(long id, HisMediOrgFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediOrgDAO.GetById(id, filter.Query());
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
