using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPackage
{
    partial class HisPackageGet : BusinessBase
    {
        internal HisPackageGet()
            : base()
        {

        }

        internal HisPackageGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PACKAGE> Get(HisPackageFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPackageDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKAGE GetById(long id)
        {
            try
            {
                return GetById(id, new HisPackageFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PACKAGE GetById(long id, HisPackageFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPackageDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PACKAGE> GetActive()
        {
            HisPackageFilterQuery filter = new HisPackageFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            return this.Get(filter);
        }
    }
}
