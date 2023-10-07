using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcdGroup
{
    public partial class TytUninfectIcdGroupDAO : EntityBase
    {
        public List<V_TYT_UNINFECT_ICD_GROUP> GetView(TytUninfectIcdGroupSO search, CommonParam param)
        {
            List<V_TYT_UNINFECT_ICD_GROUP> result = new List<V_TYT_UNINFECT_ICD_GROUP>();
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

        public V_TYT_UNINFECT_ICD_GROUP GetViewById(long id, TytUninfectIcdGroupSO search)
        {
            V_TYT_UNINFECT_ICD_GROUP result = null;

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
