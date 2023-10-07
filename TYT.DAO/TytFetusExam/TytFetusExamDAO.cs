using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusExam
{
    public partial class TytFetusExamDAO : EntityBase
    {
        private TytFetusExamGet GetWorker
        {
            get
            {
                return (TytFetusExamGet)Worker.Get<TytFetusExamGet>();
            }
        }

        public List<TYT_FETUS_EXAM> Get(TytFetusExamSO search, CommonParam param)
        {
            List<TYT_FETUS_EXAM> result = new List<TYT_FETUS_EXAM>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public TYT_FETUS_EXAM GetById(long id, TytFetusExamSO search)
        {
            TYT_FETUS_EXAM result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
