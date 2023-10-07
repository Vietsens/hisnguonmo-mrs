using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytTuberculosis
{
    public partial class TytTuberculosisDAO : EntityBase
    {
        private TytTuberculosisGet GetWorker
        {
            get
            {
                return (TytTuberculosisGet)Worker.Get<TytTuberculosisGet>();
            }
        }

        public List<TYT_TUBERCULOSIS> Get(TytTuberculosisSO search, CommonParam param)
        {
            List<TYT_TUBERCULOSIS> result = new List<TYT_TUBERCULOSIS>();
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

        public TYT_TUBERCULOSIS GetById(long id, TytTuberculosisSO search)
        {
            TYT_TUBERCULOSIS result = null;
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
