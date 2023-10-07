using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytNerves
{
    public partial class TytNervesDAO : EntityBase
    {
        public List<V_TYT_NERVES> GetView(TytNervesSO search, CommonParam param)
        {
            List<V_TYT_NERVES> result = new List<V_TYT_NERVES>();
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

        public V_TYT_NERVES GetViewById(long id, TytNervesSO search)
        {
            V_TYT_NERVES result = null;

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
