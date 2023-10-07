using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRestRetrType
{
    public partial class HisRestRetrTypeDAO : EntityBase
    {
        public List<V_HIS_REST_RETR_TYPE> GetView(HisRestRetrTypeSO search, CommonParam param)
        {
            List<V_HIS_REST_RETR_TYPE> result = new List<V_HIS_REST_RETR_TYPE>();
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

        public V_HIS_REST_RETR_TYPE GetViewById(long id, HisRestRetrTypeSO search)
        {
            V_HIS_REST_RETR_TYPE result = null;

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
