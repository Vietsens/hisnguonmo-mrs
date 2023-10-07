using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceRetyCat
{
    public partial class HisServiceRetyCatDAO : EntityBase
    {
        public List<V_HIS_SERVICE_RETY_CAT> GetView(HisServiceRetyCatSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_RETY_CAT> result = new List<V_HIS_SERVICE_RETY_CAT>();
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

        public V_HIS_SERVICE_RETY_CAT GetViewById(long id, HisServiceRetyCatSO search)
        {
            V_HIS_SERVICE_RETY_CAT result = null;

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
