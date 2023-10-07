using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterial
{
    public partial class HisMaterialDAO : EntityBase
    {
        private HisMaterialGet GetWorker
        {
            get
            {
                return (HisMaterialGet)Worker.Get<HisMaterialGet>();
            }
        }
        public List<HIS_MATERIAL> Get(HisMaterialSO search, CommonParam param)
        {
            List<HIS_MATERIAL> result = new List<HIS_MATERIAL>();
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

        public HIS_MATERIAL GetById(long id, HisMaterialSO search)
        {
            HIS_MATERIAL result = null;
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
