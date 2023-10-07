using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytKhh
{
    public partial class TytKhhDAO : EntityBase
    {
        public List<V_TYT_KHH> GetView(TytKhhSO search, CommonParam param)
        {
            List<V_TYT_KHH> result = new List<V_TYT_KHH>();
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

        public V_TYT_KHH GetViewById(long id, TytKhhSO search)
        {
            V_TYT_KHH result = null;

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
