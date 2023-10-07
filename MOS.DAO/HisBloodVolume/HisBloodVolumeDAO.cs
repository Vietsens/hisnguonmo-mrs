using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodVolume
{
    public partial class HisBloodVolumeDAO : EntityBase
    {
        private HisBloodVolumeGet GetWorker
        {
            get
            {
                return (HisBloodVolumeGet)Worker.Get<HisBloodVolumeGet>();
            }
        }
        public List<HIS_BLOOD_VOLUME> Get(HisBloodVolumeSO search, CommonParam param)
        {
            List<HIS_BLOOD_VOLUME> result = new List<HIS_BLOOD_VOLUME>();
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

        public HIS_BLOOD_VOLUME GetById(long id, HisBloodVolumeSO search)
        {
            HIS_BLOOD_VOLUME result = null;
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
