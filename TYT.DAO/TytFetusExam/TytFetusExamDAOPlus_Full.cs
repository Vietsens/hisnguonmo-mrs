using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusExam
{
    public partial class TytFetusExamDAO : EntityBase
    {
        public List<V_TYT_FETUS_EXAM> GetView(TytFetusExamSO search, CommonParam param)
        {
            List<V_TYT_FETUS_EXAM> result = new List<V_TYT_FETUS_EXAM>();

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

        public TYT_FETUS_EXAM GetByCode(string code, TytFetusExamSO search)
        {
            TYT_FETUS_EXAM result = null;

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
        
        public V_TYT_FETUS_EXAM GetViewById(long id, TytFetusExamSO search)
        {
            V_TYT_FETUS_EXAM result = null;

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

        public V_TYT_FETUS_EXAM GetViewByCode(string code, TytFetusExamSO search)
        {
            V_TYT_FETUS_EXAM result = null;

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

        public Dictionary<string, TYT_FETUS_EXAM> GetDicByCode(TytFetusExamSO search, CommonParam param)
        {
            Dictionary<string, TYT_FETUS_EXAM> result = new Dictionary<string, TYT_FETUS_EXAM>();
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
