using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBedBsty
{
    public partial class HisBedBstyDAO : EntityBase
    {
        private HisBedBstyGet GetWorker
        {
            get
            {
                return (HisBedBstyGet)Worker.Get<HisBedBstyGet>();
            }
        }
        public List<HIS_BED_BSTY> Get(HisBedBstySO search, CommonParam param)
        {
            List<HIS_BED_BSTY> result = new List<HIS_BED_BSTY>();
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

        public HIS_BED_BSTY GetById(long id, HisBedBstySO search)
        {
            HIS_BED_BSTY result = null;
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
