using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodVolume
{
    partial class HisBloodVolumeGet : BusinessBase
    {
        internal HisBloodVolumeGet()
            : base()
        {

        }

        internal HisBloodVolumeGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD_VOLUME> Get(HisBloodVolumeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodVolumeDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_VOLUME GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodVolumeFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_VOLUME GetById(long id, HisBloodVolumeFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodVolumeDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
