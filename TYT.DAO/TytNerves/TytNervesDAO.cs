using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytNerves
{
    public partial class TytNervesDAO : EntityBase
    {
        private TytNervesGet GetWorker
        {
            get
            {
                return (TytNervesGet)Worker.Get<TytNervesGet>();
            }
        }

        public List<TYT_NERVES> Get(TytNervesSO search, CommonParam param)
        {
            List<TYT_NERVES> result = new List<TYT_NERVES>();
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

        public TYT_NERVES GetById(long id, TytNervesSO search)
        {
            TYT_NERVES result = null;
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
