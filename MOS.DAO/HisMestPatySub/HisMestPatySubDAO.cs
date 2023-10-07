using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMestPatySub
{
    public partial class HisMestPatySubDAO : EntityBase
    {
        private HisMestPatySubGet GetWorker
        {
            get
            {
                return (HisMestPatySubGet)Worker.Get<HisMestPatySubGet>();
            }
        }
        public List<HIS_MEST_PATY_SUB> Get(HisMestPatySubSO search, CommonParam param)
        {
            List<HIS_MEST_PATY_SUB> result = new List<HIS_MEST_PATY_SUB>();
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

        public HIS_MEST_PATY_SUB GetById(long id, HisMestPatySubSO search)
        {
            HIS_MEST_PATY_SUB result = null;
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
