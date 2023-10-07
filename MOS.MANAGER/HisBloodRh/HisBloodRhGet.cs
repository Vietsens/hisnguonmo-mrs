using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodRh
{
    partial class HisBloodRhGet : BusinessBase
    {
        internal HisBloodRhGet()
            : base()
        {

        }

        internal HisBloodRhGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD_RH> Get(HisBloodRhFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodRhDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_RH GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodRhFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_RH GetById(long id, HisBloodRhFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodRhDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_RH GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisBloodRhFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_RH GetByCode(string code, HisBloodRhFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodRhDAO.GetByCode(code, filter.Query());
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
