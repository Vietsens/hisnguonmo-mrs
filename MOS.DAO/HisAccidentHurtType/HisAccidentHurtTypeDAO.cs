using Inventec.Common.Repository;
using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccidentHurtType
{
    public partial class HisAccidentHurtTypeDAO : EntityBase
    {
        private HisAccidentHurtTypeGet GetWorker
        {
            get
            {
                return (HisAccidentHurtTypeGet)Worker.Get<HisAccidentHurtTypeGet>();
            }
        }
        public List<HIS_ACCIDENT_HURT_TYPE> Get(HisAccidentHurtTypeSO search, CommonParam param)
        {
            List<HIS_ACCIDENT_HURT_TYPE> result = new List<HIS_ACCIDENT_HURT_TYPE>();
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

        public HIS_ACCIDENT_HURT_TYPE GetById(long id, HisAccidentHurtTypeSO search)
        {
            HIS_ACCIDENT_HURT_TYPE result = null;
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
