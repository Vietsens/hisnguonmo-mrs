using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBidBloodType
{
    public partial class HisBidBloodTypeDAO : EntityBase
    {
        private HisBidBloodTypeGet GetWorker
        {
            get
            {
                return (HisBidBloodTypeGet)Worker.Get<HisBidBloodTypeGet>();
            }
        }
        public List<HIS_BID_BLOOD_TYPE> Get(HisBidBloodTypeSO search, CommonParam param)
        {
            List<HIS_BID_BLOOD_TYPE> result = new List<HIS_BID_BLOOD_TYPE>();
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

        public HIS_BID_BLOOD_TYPE GetById(long id, HisBidBloodTypeSO search)
        {
            HIS_BID_BLOOD_TYPE result = null;
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
