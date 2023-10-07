using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusBorn
{
    public partial class TytFetusBornDAO : EntityBase
    {
        public List<V_TYT_FETUS_BORN> GetView(TytFetusBornSO search, CommonParam param)
        {
            List<V_TYT_FETUS_BORN> result = new List<V_TYT_FETUS_BORN>();
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

        public V_TYT_FETUS_BORN GetViewById(long id, TytFetusBornSO search)
        {
            V_TYT_FETUS_BORN result = null;

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
