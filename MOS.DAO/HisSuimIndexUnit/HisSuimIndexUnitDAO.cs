using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSuimIndexUnit
{
    public partial class HisSuimIndexUnitDAO : EntityBase
    {
        private HisSuimIndexUnitGet GetWorker
        {
            get
            {
                return (HisSuimIndexUnitGet)Worker.Get<HisSuimIndexUnitGet>();
            }
        }
        public List<HIS_SUIM_INDEX_UNIT> Get(HisSuimIndexUnitSO search, CommonParam param)
        {
            List<HIS_SUIM_INDEX_UNIT> result = new List<HIS_SUIM_INDEX_UNIT>();
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

        public HIS_SUIM_INDEX_UNIT GetById(long id, HisSuimIndexUnitSO search)
        {
            HIS_SUIM_INDEX_UNIT result = null;
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
