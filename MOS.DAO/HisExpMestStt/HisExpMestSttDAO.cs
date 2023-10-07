using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestStt
{
    public partial class HisExpMestSttDAO : EntityBase
    {
        private HisExpMestSttGet GetWorker
        {
            get
            {
                return (HisExpMestSttGet)Worker.Get<HisExpMestSttGet>();
            }
        }
        public List<HIS_EXP_MEST_STT> Get(HisExpMestSttSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_STT> result = new List<HIS_EXP_MEST_STT>();
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

        public HIS_EXP_MEST_STT GetById(long id, HisExpMestSttSO search)
        {
            HIS_EXP_MEST_STT result = null;
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
