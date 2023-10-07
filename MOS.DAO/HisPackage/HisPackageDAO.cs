using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPackage
{
    public partial class HisPackageDAO : EntityBase
    {
        private HisPackageGet GetWorker
        {
            get
            {
                return (HisPackageGet)Worker.Get<HisPackageGet>();
            }
        }
        public List<HIS_PACKAGE> Get(HisPackageSO search, CommonParam param)
        {
            List<HIS_PACKAGE> result = new List<HIS_PACKAGE>();
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

        public HIS_PACKAGE GetById(long id, HisPackageSO search)
        {
            HIS_PACKAGE result = null;
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
