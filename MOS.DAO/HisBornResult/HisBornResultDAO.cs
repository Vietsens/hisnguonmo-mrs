using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBornResult
{
    public partial class HisBornResultDAO : EntityBase
    {
        private HisBornResultGet GetWorker
        {
            get
            {
                return (HisBornResultGet)Worker.Get<HisBornResultGet>();
            }
        }
        public List<HIS_BORN_RESULT> Get(HisBornResultSO search, CommonParam param)
        {
            List<HIS_BORN_RESULT> result = new List<HIS_BORN_RESULT>();
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

        public HIS_BORN_RESULT GetById(long id, HisBornResultSO search)
        {
            HIS_BORN_RESULT result = null;
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
