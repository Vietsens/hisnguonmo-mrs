using TYT.DAO.StagingObject;
using TYT.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace TYT.DAO.TytKhh
{
    public partial class TytKhhDAO : EntityBase
    {
        private TytKhhGet GetWorker
        {
            get
            {
                return (TytKhhGet)Worker.Get<TytKhhGet>();
            }
        }

        public List<TYT_KHH> Get(TytKhhSO search, CommonParam param)
        {
            List<TYT_KHH> result = new List<TYT_KHH>();
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

        public TYT_KHH GetById(long id, TytKhhSO search)
        {
            TYT_KHH result = null;
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
