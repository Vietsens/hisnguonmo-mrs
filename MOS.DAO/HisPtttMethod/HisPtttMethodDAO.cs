using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisPtttMethod
{
    public partial class HisPtttMethodDAO : EntityBase
    {
        private HisPtttMethodGet GetWorker
        {
            get
            {
                return (HisPtttMethodGet)Worker.Get<HisPtttMethodGet>();
            }
        }
        public List<HIS_PTTT_METHOD> Get(HisPtttMethodSO search, CommonParam param)
        {
            List<HIS_PTTT_METHOD> result = new List<HIS_PTTT_METHOD>();
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

        public HIS_PTTT_METHOD GetById(long id, HisPtttMethodSO search)
        {
            HIS_PTTT_METHOD result = null;
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
