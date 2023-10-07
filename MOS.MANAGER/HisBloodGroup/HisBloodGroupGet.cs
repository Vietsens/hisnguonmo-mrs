using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodGroup
{
    partial class HisBloodGroupGet : BusinessBase
    {
        internal HisBloodGroupGet()
            : base()
        {

        }

        internal HisBloodGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD_GROUP> Get(HisBloodGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_GROUP GetById(long id, HisBloodGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodGroupDAO.GetById(id, filter.Query());
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
