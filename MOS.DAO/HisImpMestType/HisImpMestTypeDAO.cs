using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestType
{
    public partial class HisImpMestTypeDAO : EntityBase
    {
        private HisImpMestTypeGet GetWorker
        {
            get
            {
                return (HisImpMestTypeGet)Worker.Get<HisImpMestTypeGet>();
            }
        }
        public List<HIS_IMP_MEST_TYPE> Get(HisImpMestTypeSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_TYPE> result = new List<HIS_IMP_MEST_TYPE>();
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

        public HIS_IMP_MEST_TYPE GetById(long id, HisImpMestTypeSO search)
        {
            HIS_IMP_MEST_TYPE result = null;
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
