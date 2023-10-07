using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBornPosition
{
    public partial class HisBornPositionDAO : EntityBase
    {
        private HisBornPositionGet GetWorker
        {
            get
            {
                return (HisBornPositionGet)Worker.Get<HisBornPositionGet>();
            }
        }
        public List<HIS_BORN_POSITION> Get(HisBornPositionSO search, CommonParam param)
        {
            List<HIS_BORN_POSITION> result = new List<HIS_BORN_POSITION>();
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

        public HIS_BORN_POSITION GetById(long id, HisBornPositionSO search)
        {
            HIS_BORN_POSITION result = null;
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
