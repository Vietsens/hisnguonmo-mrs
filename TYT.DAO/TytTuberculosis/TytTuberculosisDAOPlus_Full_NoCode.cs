using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytTuberculosis
{
    public partial class TytTuberculosisDAO : EntityBase
    {
        public List<V_TYT_TUBERCULOSIS> GetView(TytTuberculosisSO search, CommonParam param)
        {
            List<V_TYT_TUBERCULOSIS> result = new List<V_TYT_TUBERCULOSIS>();
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

        public V_TYT_TUBERCULOSIS GetViewById(long id, TytTuberculosisSO search)
        {
            V_TYT_TUBERCULOSIS result = null;

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
