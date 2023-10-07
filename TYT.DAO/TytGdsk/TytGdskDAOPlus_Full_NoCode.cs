using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytGdsk
{
    public partial class TytGdskDAO : EntityBase
    {
        public List<V_TYT_GDSK> GetView(TytGdskSO search, CommonParam param)
        {
            List<V_TYT_GDSK> result = new List<V_TYT_GDSK>();
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

        public V_TYT_GDSK GetViewById(long id, TytGdskSO search)
        {
            V_TYT_GDSK result = null;

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
