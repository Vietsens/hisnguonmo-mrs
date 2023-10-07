using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytFetusBorn
{
    public partial class TytFetusBornDAO : EntityBase
    {
        private TytFetusBornGet GetWorker
        {
            get
            {
                return (TytFetusBornGet)Worker.Get<TytFetusBornGet>();
            }
        }

        public List<TYT_FETUS_BORN> Get(TytFetusBornSO search, CommonParam param)
        {
            List<TYT_FETUS_BORN> result = new List<TYT_FETUS_BORN>();
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

        public TYT_FETUS_BORN GetById(long id, TytFetusBornSO search)
        {
            TYT_FETUS_BORN result = null;
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
