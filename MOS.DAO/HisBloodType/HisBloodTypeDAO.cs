using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodType
{
    public partial class HisBloodTypeDAO : EntityBase
    {
        private HisBloodTypeGet GetWorker
        {
            get
            {
                return (HisBloodTypeGet)Worker.Get<HisBloodTypeGet>();
            }
        }
        public List<HIS_BLOOD_TYPE> Get(HisBloodTypeSO search, CommonParam param)
        {
            List<HIS_BLOOD_TYPE> result = new List<HIS_BLOOD_TYPE>();
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

        public HIS_BLOOD_TYPE GetById(long id, HisBloodTypeSO search)
        {
            HIS_BLOOD_TYPE result = null;
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
