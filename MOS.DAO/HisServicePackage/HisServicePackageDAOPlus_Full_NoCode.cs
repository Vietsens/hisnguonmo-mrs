using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServicePackage
{
    public partial class HisServicePackageDAO : EntityBase
    {
        public List<V_HIS_SERVICE_PACKAGE> GetView(HisServicePackageSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_PACKAGE> result = new List<V_HIS_SERVICE_PACKAGE>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_SERVICE_PACKAGE GetViewById(long id, HisServicePackageSO search)
        {
            V_HIS_SERVICE_PACKAGE result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
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
