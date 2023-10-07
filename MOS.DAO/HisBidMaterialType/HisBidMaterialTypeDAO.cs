using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidMaterialType
{
    public partial class HisBidMaterialTypeDAO : EntityBase
    {
        private HisBidMaterialTypeGet GetWorker
        {
            get
            {
                return (HisBidMaterialTypeGet)Worker.Get<HisBidMaterialTypeGet>();
            }
        }
        public List<HIS_BID_MATERIAL_TYPE> Get(HisBidMaterialTypeSO search, CommonParam param)
        {
            List<HIS_BID_MATERIAL_TYPE> result = new List<HIS_BID_MATERIAL_TYPE>();
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

        public HIS_BID_MATERIAL_TYPE GetById(long id, HisBidMaterialTypeSO search)
        {
            HIS_BID_MATERIAL_TYPE result = null;
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
