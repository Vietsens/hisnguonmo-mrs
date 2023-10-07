using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisService
{
    public partial class HisServiceDAO : EntityBase
    {
        public List<V_HIS_SERVICE> GetView(HisServiceSO search, CommonParam param)
        {
            List<V_HIS_SERVICE> result = new List<V_HIS_SERVICE>();
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

        public V_HIS_SERVICE GetViewById(long id, HisServiceSO search)
        {
            V_HIS_SERVICE result = null;

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

        public List<V_HIS_SERVICE_1> GetView1(HisServiceSO search, CommonParam param)
        {
            List<V_HIS_SERVICE_1> result = new List<V_HIS_SERVICE_1>();
            try
            {
                result = GetWorker.GetView1(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_SERVICE_1 GetView1ById(long id, HisServiceSO search)
        {
            V_HIS_SERVICE_1 result = null;

            try
            {
                result = GetWorker.GetView1ById(id, search);
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
