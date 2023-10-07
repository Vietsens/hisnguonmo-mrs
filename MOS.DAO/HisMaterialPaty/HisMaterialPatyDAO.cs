using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterialPaty
{
    public partial class HisMaterialPatyDAO : EntityBase
    {
        private HisMaterialPatyGet GetWorker
        {
            get
            {
                return (HisMaterialPatyGet)Worker.Get<HisMaterialPatyGet>();
            }
        }
        public List<HIS_MATERIAL_PATY> Get(HisMaterialPatySO search, CommonParam param)
        {
            List<HIS_MATERIAL_PATY> result = new List<HIS_MATERIAL_PATY>();
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

        public HIS_MATERIAL_PATY GetById(long id, HisMaterialPatySO search)
        {
            HIS_MATERIAL_PATY result = null;
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
