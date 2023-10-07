using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceType
{
    public partial class HisServiceTypeDAO : EntityBase
    {
        private HisServiceTypeGet GetWorker
        {
            get
            {
                return (HisServiceTypeGet)Worker.Get<HisServiceTypeGet>();
            }
        }
        public List<HIS_SERVICE_TYPE> Get(HisServiceTypeSO search, CommonParam param)
        {
            List<HIS_SERVICE_TYPE> result = new List<HIS_SERVICE_TYPE>();
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

        public HIS_SERVICE_TYPE GetById(long id, HisServiceTypeSO search)
        {
            HIS_SERVICE_TYPE result = null;
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
