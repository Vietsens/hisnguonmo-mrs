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
    }
}
