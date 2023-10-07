using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePackage
{
    class HisServicePackageGet : GetBase
    {
        internal HisServicePackageGet()
            : base()
        {

        }

        internal HisServicePackageGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERVICE_PACKAGE> Get(HisServicePackageFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePackageDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERVICE_PACKAGE> GetView(HisServicePackageViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePackageDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_PACKAGE GetById(long id)
        {
            try
            {
                return GetById(id, new HisServicePackageFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERVICE_PACKAGE GetById(long id, HisServicePackageFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePackageDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_PACKAGE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisServicePackageViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERVICE_PACKAGE GetViewById(long id, HisServicePackageViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisServicePackageDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERVICE_PACKAGE> GetByServiceId(long serviceId)
        {
            try
            {
                HisServicePackageFilterQuery filter = new HisServicePackageFilterQuery();
                filter.SERVICE_ID = serviceId;
                return this.Get(filter);
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
