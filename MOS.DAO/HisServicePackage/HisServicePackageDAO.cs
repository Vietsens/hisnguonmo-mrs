using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServicePackage
{
    public partial class HisServicePackageDAO : EntityBase
    {
        private HisServicePackageGet GetWorker
        {
            get
            {
                return (HisServicePackageGet)Worker.Get<HisServicePackageGet>();
            }
        }

        public List<HIS_SERVICE_PACKAGE> Get(HisServicePackageSO search, CommonParam param)
        {
            List<HIS_SERVICE_PACKAGE> result = new List<HIS_SERVICE_PACKAGE>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_SERVICE_PACKAGE GetById(long id, HisServicePackageSO search)
        {
            HIS_SERVICE_PACKAGE result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
