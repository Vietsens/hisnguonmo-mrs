using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytHiv
{
    public partial class TytHivDAO : EntityBase
    {
        public List<V_TYT_HIV> GetView(TytHivSO search, CommonParam param)
        {
            List<V_TYT_HIV> result = new List<V_TYT_HIV>();
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

        public V_TYT_HIV GetViewById(long id, TytHivSO search)
        {
            V_TYT_HIV result = null;

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
