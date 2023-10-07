using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCareType
{
    public partial class HisCareTypeDAO : EntityBase
    {
        private HisCareTypeGet GetWorker
        {
            get
            {
                return (HisCareTypeGet)Worker.Get<HisCareTypeGet>();
            }
        }
        public List<HIS_CARE_TYPE> Get(HisCareTypeSO search, CommonParam param)
        {
            List<HIS_CARE_TYPE> result = new List<HIS_CARE_TYPE>();
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

        public HIS_CARE_TYPE GetById(long id, HisCareTypeSO search)
        {
            HIS_CARE_TYPE result = null;
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
