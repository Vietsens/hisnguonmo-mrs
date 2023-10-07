using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmteMaterialType
{
    public partial class HisEmteMaterialTypeDAO : EntityBase
    {
        private HisEmteMaterialTypeGet GetWorker
        {
            get
            {
                return (HisEmteMaterialTypeGet)Worker.Get<HisEmteMaterialTypeGet>();
            }
        }
        public List<HIS_EMTE_MATERIAL_TYPE> Get(HisEmteMaterialTypeSO search, CommonParam param)
        {
            List<HIS_EMTE_MATERIAL_TYPE> result = new List<HIS_EMTE_MATERIAL_TYPE>();
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

        public HIS_EMTE_MATERIAL_TYPE GetById(long id, HisEmteMaterialTypeSO search)
        {
            HIS_EMTE_MATERIAL_TYPE result = null;
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
