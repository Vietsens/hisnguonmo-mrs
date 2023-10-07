using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisService
{
    public partial class HisServiceDAO : EntityBase
    {
        private HisServiceGet GetWorker
        {
            get
            {
                return (HisServiceGet)Worker.Get<HisServiceGet>();
            }
        }
        public List<HIS_SERVICE> Get(HisServiceSO search, CommonParam param)
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
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

        public HIS_SERVICE GetById(long id, HisServiceSO search)
        {
            HIS_SERVICE result = null;
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
