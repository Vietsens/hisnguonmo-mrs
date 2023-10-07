using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHurt
{
    public partial class HisAccidentHurtDAO : EntityBase
    {
        private HisAccidentHurtGet GetWorker
        {
            get
            {
                return (HisAccidentHurtGet)Worker.Get<HisAccidentHurtGet>();
            }
        }

        public List<HIS_ACCIDENT_HURT> Get(HisAccidentHurtSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_HURT> result = new List<HIS_ACCIDENT_HURT>();
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

        public HIS_ACCIDENT_HURT GetById(long id, HisAccidentHurtSO search)
        {
            HIS_ACCIDENT_HURT result = null;
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
