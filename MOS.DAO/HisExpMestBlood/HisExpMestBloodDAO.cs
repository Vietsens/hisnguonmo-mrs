using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestBlood
{
    public partial class HisExpMestBloodDAO : EntityBase
    {
        private HisExpMestBloodGet GetWorker
        {
            get
            {
                return (HisExpMestBloodGet)Worker.Get<HisExpMestBloodGet>();
            }
        }
        public List<HIS_EXP_MEST_BLOOD> Get(HisExpMestBloodSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_BLOOD> result = new List<HIS_EXP_MEST_BLOOD>();
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

        public HIS_EXP_MEST_BLOOD GetById(long id, HisExpMestBloodSO search)
        {
            HIS_EXP_MEST_BLOOD result = null;
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
