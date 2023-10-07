using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMestMaterial
{
    public partial class HisImpMestMaterialDAO : EntityBase
    {
        private HisImpMestMaterialGet GetWorker
        {
            get
            {
                return (HisImpMestMaterialGet)Worker.Get<HisImpMestMaterialGet>();
            }
        }
        public List<HIS_IMP_MEST_MATERIAL> Get(HisImpMestMaterialSO search, CommonParam param)
        {
            List<HIS_IMP_MEST_MATERIAL> result = new List<HIS_IMP_MEST_MATERIAL>();
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

        public HIS_IMP_MEST_MATERIAL GetById(long id, HisImpMestMaterialSO search)
        {
            HIS_IMP_MEST_MATERIAL result = null;
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
