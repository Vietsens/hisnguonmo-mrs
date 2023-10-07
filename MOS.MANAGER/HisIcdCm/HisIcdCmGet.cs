using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdCm
{
    partial class HisIcdCmGet : BusinessBase
    {
        internal HisIcdCmGet()
            : base()
        {

        }

        internal HisIcdCmGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ICD_CM> Get(HisIcdCmFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdCmDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_CM GetById(long id)
        {
            try
            {
                return GetById(id, new HisIcdCmFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ICD_CM GetById(long id, HisIcdCmFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisIcdCmDAO.GetById(id, filter.Query());
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
