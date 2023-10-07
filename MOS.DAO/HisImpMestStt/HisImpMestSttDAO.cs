using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestStt
{
    public partial class HisImpMestSttDAO : EntityBase
    {
        private HisImpMestSttGet GetWorker
        {
            get
            {
                return (HisImpMestSttGet)Worker.Get<HisImpMestSttGet>();
            }
        }
        public List<HIS_IMP_MEST_STT> Get(HisImpMestSttSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_STT> result = new List<HIS_IMP_MEST_STT>();
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

        public HIS_IMP_MEST_STT GetById(long id, HisImpMestSttSO search)
        {
            HIS_IMP_MEST_STT result = null;
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
