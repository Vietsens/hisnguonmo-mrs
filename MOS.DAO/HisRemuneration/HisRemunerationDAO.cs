using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisRemuneration
{
    public partial class HisRemunerationDAO : EntityBase
    {
        private HisRemunerationGet GetWorker
        {
            get
            {
                return (HisRemunerationGet)Worker.Get<HisRemunerationGet>();
            }
        }
        public List<HIS_REMUNERATION> Get(HisRemunerationSO search, CommonParam param)
        {
            List<HIS_REMUNERATION> result = new List<HIS_REMUNERATION>();
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

        public HIS_REMUNERATION GetById(long id, HisRemunerationSO search)
        {
            HIS_REMUNERATION result = null;
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
