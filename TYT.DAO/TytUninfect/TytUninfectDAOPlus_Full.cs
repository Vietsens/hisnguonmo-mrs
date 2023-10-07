using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytUninfect
{
    public partial class TytUninfectDAO : EntityBase
    {
        public List<V_TYT_UNINFECT> GetView(TytUninfectSO search, CommonParam param)
        {
            List<V_TYT_UNINFECT> result = new List<V_TYT_UNINFECT>();

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

        public TYT_UNINFECT GetByCode(string code, TytUninfectSO search)
        {
            TYT_UNINFECT result = null;

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
        
        public V_TYT_UNINFECT GetViewById(long id, TytUninfectSO search)
        {
            V_TYT_UNINFECT result = null;

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

        public V_TYT_UNINFECT GetViewByCode(string code, TytUninfectSO search)
        {
            V_TYT_UNINFECT result = null;

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

        public Dictionary<string, TYT_UNINFECT> GetDicByCode(TytUninfectSO search, CommonParam param)
        {
            Dictionary<string, TYT_UNINFECT> result = new Dictionary<string, TYT_UNINFECT>();
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
