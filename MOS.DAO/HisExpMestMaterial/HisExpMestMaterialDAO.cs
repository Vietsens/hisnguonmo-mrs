using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMaterial
{
    public partial class HisExpMestMaterialDAO : EntityBase
    {
        private HisExpMestMaterialGet GetWorker
        {
            get
            {
                return (HisExpMestMaterialGet)Worker.Get<HisExpMestMaterialGet>();
            }
        }
        public List<HIS_EXP_MEST_MATERIAL> Get(HisExpMestMaterialSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_MATERIAL> result = new List<HIS_EXP_MEST_MATERIAL>();
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

        public HIS_EXP_MEST_MATERIAL GetById(long id, HisExpMestMaterialSO search)
        {
            HIS_EXP_MEST_MATERIAL result = null;
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
