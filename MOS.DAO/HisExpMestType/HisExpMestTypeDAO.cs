using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestType
{
    public partial class HisExpMestTypeDAO : EntityBase
    {
        private HisExpMestTypeGet GetWorker
        {
            get
            {
                return (HisExpMestTypeGet)Worker.Get<HisExpMestTypeGet>();
            }
        }
        public List<HIS_EXP_MEST_TYPE> Get(HisExpMestTypeSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_TYPE> result = new List<HIS_EXP_MEST_TYPE>();
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

        public HIS_EXP_MEST_TYPE GetById(long id, HisExpMestTypeSO search)
        {
            HIS_EXP_MEST_TYPE result = null;
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
