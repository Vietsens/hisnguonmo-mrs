using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisSuimSetySuin
{
    public partial class HisSuimSetySuinDAO : EntityBase
    {
        private HisSuimSetySuinGet GetWorker
        {
            get
            {
                return (HisSuimSetySuinGet)Worker.Get<HisSuimSetySuinGet>();
            }
        }
        public List<HIS_SUIM_SETY_SUIN> Get(HisSuimSetySuinSO search, CommonParam param)
        {
            List<HIS_SUIM_SETY_SUIN> result = new List<HIS_SUIM_SETY_SUIN>();
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

        public HIS_SUIM_SETY_SUIN GetById(long id, HisSuimSetySuinSO search)
        {
            HIS_SUIM_SETY_SUIN result = null;
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
