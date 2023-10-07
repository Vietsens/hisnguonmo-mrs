using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterialType
{
    public partial class HisMaterialTypeDAO : EntityBase
    {
        private HisMaterialTypeGet GetWorker
        {
            get
            {
                return (HisMaterialTypeGet)Worker.Get<HisMaterialTypeGet>();
            }
        }
        public List<HIS_MATERIAL_TYPE> Get(HisMaterialTypeSO search, CommonParam param)
        {
            List<HIS_MATERIAL_TYPE> result = new List<HIS_MATERIAL_TYPE>();
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

        public HIS_MATERIAL_TYPE GetById(long id, HisMaterialTypeSO search)
        {
            HIS_MATERIAL_TYPE result = null;
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
