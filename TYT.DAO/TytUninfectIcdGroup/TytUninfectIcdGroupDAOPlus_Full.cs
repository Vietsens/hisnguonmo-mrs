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

        public TYT_UNINFECT_ICD_GROUP GetByCode(string code, TytUninfectIcdGroupSO search)
        {
            TYT_UNINFECT_ICD_GROUP result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
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

        public V_TYT_UNINFECT_ICD_GROUP GetViewByCode(string code, TytUninfectIcdGroupSO search)
        {
            V_TYT_UNINFECT_ICD_GROUP result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, TYT_UNINFECT_ICD_GROUP> GetDicByCode(TytUninfectIcdGroupSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT_ICD_GROUP> result = new Dictionary<string, TYT_UNINFECT_ICD_GROUP>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public bool ExistsCode(string code, long? id)
        {
            try
            {
                return CheckWorker.ExistsCode(code, id);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                throw;
            }
        }
    }
}
