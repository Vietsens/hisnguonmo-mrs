using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareDetail
{
    public partial class HisCareDetailDAO : EntityBase
    {
        private HisCareDetailGet GetWorker
        {
            get
            {
                return (HisCareDetailGet)Worker.Get<HisCareDetailGet>();
            }
        }
        public List<HIS_CARE_DETAIL> Get(HisCareDetailSO search, CommonParam param)
        {
            List<HIS_CARE_DETAIL> result = new List<HIS_CARE_DETAIL>();
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

        public HIS_CARE_DETAIL GetById(long id, HisCareDetailSO search)
        {
            HIS_CARE_DETAIL result = null;
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
