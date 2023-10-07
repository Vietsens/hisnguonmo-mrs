using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisHeinServiceType
{
    public partial class HisHeinServiceTypeDAO : EntityBase
    {
        private HisHeinServiceTypeGet GetWorker
        {
            get
            {
                return (HisHeinServiceTypeGet)Worker.Get<HisHeinServiceTypeGet>();
            }
        }
        public List<HIS_HEIN_SERVICE_TYPE> Get(HisHeinServiceTypeSO search, CommonParam param)
        {
            List<HIS_HEIN_SERVICE_TYPE> result = new List<HIS_HEIN_SERVICE_TYPE>();
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

        public HIS_HEIN_SERVICE_TYPE GetById(long id, HisHeinServiceTypeSO search)
        {
            HIS_HEIN_SERVICE_TYPE result = null;
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
