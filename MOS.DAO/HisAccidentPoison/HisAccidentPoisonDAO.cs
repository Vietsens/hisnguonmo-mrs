using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentPoison
{
    public partial class HisAccidentPoisonDAO : EntityBase
    {
        private HisAccidentPoisonGet GetWorker
        {
            get
            {
                return (HisAccidentPoisonGet)Worker.Get<HisAccidentPoisonGet>();
            }
        }
        public List<HIS_ACCIDENT_POISON> Get(HisAccidentPoisonSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_POISON> result = new List<HIS_ACCIDENT_POISON>();
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

        public HIS_ACCIDENT_POISON GetById(long id, HisAccidentPoisonSO search)
        {
            HIS_ACCIDENT_POISON result = null;
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
