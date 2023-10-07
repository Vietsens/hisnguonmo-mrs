using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSuimIndex
{
    public partial class HisSuimIndexDAO : EntityBase
    {
        private HisSuimIndexGet GetWorker
        {
            get
            {
                return (HisSuimIndexGet)Worker.Get<HisSuimIndexGet>();
            }
        }
        public List<HIS_SUIM_INDEX> Get(HisSuimIndexSO search, CommonParam param)
        {
            List<HIS_SUIM_INDEX> result = new List<HIS_SUIM_INDEX>();
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

        public HIS_SUIM_INDEX GetById(long id, HisSuimIndexSO search)
        {
            HIS_SUIM_INDEX result = null;
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
