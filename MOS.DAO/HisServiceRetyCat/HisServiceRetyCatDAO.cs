using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisServiceRetyCat
{
    public partial class HisServiceRetyCatDAO : EntityBase
    {
        private HisServiceRetyCatGet GetWorker
        {
            get
            {
                return (HisServiceRetyCatGet)Worker.Get<HisServiceRetyCatGet>();
            }
        }
        public List<HIS_SERVICE_RETY_CAT> Get(HisServiceRetyCatSO search, CommonParam param)
        {
            List<HIS_SERVICE_RETY_CAT> result = new List<HIS_SERVICE_RETY_CAT>();
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

        public HIS_SERVICE_RETY_CAT GetById(long id, HisServiceRetyCatSO search)
        {
            HIS_SERVICE_RETY_CAT result = null;
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
