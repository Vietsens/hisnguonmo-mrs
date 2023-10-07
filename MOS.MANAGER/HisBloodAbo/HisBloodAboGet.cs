using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodAbo
{
    partial class HisBloodAboGet : BusinessBase
    {
        internal HisBloodAboGet()
            : base()
        {

        }

        internal HisBloodAboGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_BLOOD_ABO> Get(HisBloodAboFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodAboDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_ABO GetById(long id)
        {
            try
            {
                return GetById(id, new HisBloodAboFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_BLOOD_ABO GetById(long id, HisBloodAboFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodAboDAO.GetById(id, filter.Query());
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
