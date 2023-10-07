using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfectIcd
{
    public partial class TytUninfectIcdDAO : EntityBase
    {
        public List<V_TYT_UNINFECT_ICD> GetView(TytUninfectIcdSO search, CommonParam param)
        {
            List<V_TYT_UNINFECT_ICD> result = new List<V_TYT_UNINFECT_ICD>();

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

        public TYT_UNINFECT_ICD GetByCode(string code, TytUninfectIcdSO search)
        {
            TYT_UNINFECT_ICD result = null;

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
        
        public V_TYT_UNINFECT_ICD GetViewById(long id, TytUninfectIcdSO search)
        {
            V_TYT_UNINFECT_ICD result = null;

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

        public V_TYT_UNINFECT_ICD GetViewByCode(string code, TytUninfectIcdSO search)
        {
            V_TYT_UNINFECT_ICD result = null;

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

        public Dictionary<string, TYT_UNINFECT_ICD> GetDicByCode(TytUninfectIcdSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT_ICD> result = new Dictionary<string, TYT_UNINFECT_ICD>();
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
